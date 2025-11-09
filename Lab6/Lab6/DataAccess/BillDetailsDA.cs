using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BillDetailsDA
    {
        private string connectionString = Ultilities.ConnectionString;

        public List<BillDetails> GetAll()
        {
            List<BillDetails> billDetailsList = new List<BillDetails>();
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.BillDetails_GetAll;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BillDetails billDetails = new BillDetails
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                                FoodID = Convert.ToInt32(reader["FoodID"]),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            };
                            billDetailsList.Add(billDetails);
                        }
                    }
                }
            }
            return billDetailsList;
        }

        public int InsertUpdateDelete(BillDetails billDetails, int action)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.BillDetails_InsertUpdateDelete;

                    SqlParameter idParameter = new SqlParameter("@ID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = billDetails.ID
                    };
                    command.Parameters.Add(idParameter);

                    command.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = billDetails.InvoiceID;
                    command.Parameters.Add("@FoodID", SqlDbType.Int).Value = billDetails.FoodID;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = billDetails.Quantity;
                    command.Parameters.Add("@Action", SqlDbType.Int).Value = action;

                    int result = command.ExecuteNonQuery();
                    if (result > 0 && action == 0)
                    {
                        return (int)command.Parameters["@ID"].Value;
                    }
                    return result;
                }
            }
        }

        public List<BillDetails> GetBillDetails(int billId)
        {
            if (billId <= 0)
                throw new ArgumentException("Invalid Bill ID provided.");

            // Tạo danh sách để lưu các chi tiết hóa đơn
            List<BillDetails> billDetails = new List<BillDetails>();

            // Chuỗi kết nối và câu lệnh SQL để lấy dữ liệu
            string query = "SELECT Food.Name AS FoodName, Food.Price, BillDetails.Quantity, " +
                           "(Food.Price * BillDetails.Quantity) AS Total " +
                           "FROM BillDetails " +
                           "JOIN Food ON BillDetails.FoodID = Food.ID " +
                           "WHERE BillDetails.InvoiceID = @BillID"; // Thay đổi 'InvoiceID' nếu cần

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BillID", billId);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var detail = new BillDetails
                        {
                            FoodID = billId,
                            Price = Convert.ToDecimal(reader["Price"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            FoodName = reader["FoodName"].ToString()
                        };

                        billDetails.Add(detail);
                    }
                }
            }

            return billDetails;
        }
    }
}
