using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AccountDA
    {
        private string connectionString = Ultilities.ConnectionString;

        public List<Account> GetAll()
        {
            List<Account> accounts = new List<Account>();
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.Account_GetAll;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                AccountName = reader["AccountName"].ToString(),
                                Password = reader["Password"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                Tell = reader["Tell"] != DBNull.Value ? reader["Tell"].ToString() : null,
                                DateCreated = reader["DateCreated"] != DBNull.Value ? Convert.ToDateTime(reader["DateCreated"]) : DateTime.MinValue
                            };
                            accounts.Add(account);
                        }
                    }
                }
            }
            return accounts;
        }

        public int InsertUpdateDelete(Account account, int action)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.Account_InsertUpdateDelete;

                    command.Parameters.Add("@AccountName", SqlDbType.NVarChar, 100).Value = account.AccountName;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 200).Value = account.Password;
                    command.Parameters.Add("@FullName", SqlDbType.NVarChar, 1000).Value = account.FullName;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 1000).Value = account.Email as object ?? DBNull.Value;
                    command.Parameters.Add("@Tell", SqlDbType.NVarChar, 200).Value = account.Tell as object ?? DBNull.Value;
                    command.Parameters.Add("@DateCreated", SqlDbType.SmallDateTime).Value = account.DateCreated as object ?? DBNull.Value;
                    command.Parameters.Add("@Action", SqlDbType.Int).Value = action;

                    return command.ExecuteNonQuery();
                }
            }
        }

        public bool CheckAccountExists(string accountName)
        {
            string query = "SELECT COUNT(1) FROM Account WHERE AccountName = @AccountName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AccountName", accountName);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
