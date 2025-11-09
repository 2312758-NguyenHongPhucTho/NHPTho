using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
   public class FoodDA
    {
        private string connectionString = Ultilities.ConnectionString;

        /// <summary>
        /// Phương thức lấy tất cả các món ăn.
        /// </summary>
        /// <returns>Danh sách các đối tượng Food</returns>
        public List<Food> GetAll()
        {
            List<Food> foods = new List<Food>();
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.Food_GetAll;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Food food = new Food
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Unit = reader["Unit"].ToString(),
                                FoodCategoryID = Convert.ToInt32(reader["FoodCategoryID"]),
                                Price = Convert.ToInt32(reader["Price"]),
                                Notes = reader["Notes"].ToString()
                            };
                            foods.Add(food);
                        }
                    }
                }
            }
            return foods;
        }

        /// <summary>
        /// Thêm, cập nhật, hoặc xoá món ăn.
        /// </summary>
        /// <param name="food">Đối tượng Food chứa dữ liệu</param>
        /// <param name="action">1: Insert, 2: Update, 3: Delete</param>
        /// <returns>Trả về ID của món ăn (hoặc 0 nếu thất bại)</returns>
        public int InsertUpdateDelete(Food food, int action)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("Food_InsertUpdateDelete", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Thêm tham số bắt buộc
                cmd.Parameters.AddWithValue("@ID", food.ID);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@Name", food.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Unit", food.Unit ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@FoodCategoryID", food.FoodCategoryID);
                cmd.Parameters.AddWithValue("@Price", food.Price);
                cmd.Parameters.AddWithValue("@Notes", food.Notes ?? (object)DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public Food GetByName(string name)
        {
            Food food = null; // Sử dụng nullable type cho food
            string query = "SELECT * FROM Food WHERE Name = @Name";

            using (SqlConnection conn = new SqlConnection(Ultilities.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        food = new Food
                        {
                            ID = reader["ID"] != DBNull.Value ? (int)reader["ID"] : 0,
                            Name = reader["Name"]?.ToString() ?? "N/A", // Đảm bảo Name không null
                            Price = reader["Price"] != DBNull.Value ? (int)reader["Price"] : 0,
                            FoodCategoryID = reader["FoodCategoryID"] != DBNull.Value ? (int)reader["FoodCategoryID"] : 0,
                            Notes = reader["Notes"]?.ToString() ?? string.Empty // Đảm bảo Notes không null
                        };
                    }
                }
            }

            return food;
        }
    }
}
