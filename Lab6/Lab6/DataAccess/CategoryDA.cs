using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDA
    {
        private string connectionString = Ultilities.ConnectionString;

        /// <summary>
        /// Phương thức lấy tất cả các danh mục.
        /// </summary>
        /// <returns>Danh sách các đối tượng Category</returns>
        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.Category_GetAll;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["Name"].ToString(),
                                Type = Convert.ToInt32(reader["Type"])
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            return categories;
        }

        /// <summary>
        /// Thêm, cập nhật, hoặc xoá danh mục.
        /// </summary>
        /// <param name="category">Đối tượng Category chứa dữ liệu</param>
        /// <param name="action">1: Insert, 2: Update, 3: Delete</param>
        /// <returns>Trả về ID của danh mục (hoặc 0 nếu thất bại)</returns>
        public int InsertUpdateDelete(Category category, int action)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (SqlCommand command = sqlConn.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = Ultilities.Category_InsertUpdateDelete;

                    SqlParameter idParameter = new SqlParameter("@ID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Value = category.ID > 0 ? category.ID : (object)DBNull.Value
                    };
                    command.Parameters.Add(idParameter);

                    // Thêm các tham số bắt buộc
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 1000) { Value = category.Name ?? (object)DBNull.Value });
                    command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int) { Value = category.Type });
                    command.Parameters.Add(new SqlParameter("@Action", SqlDbType.Int) { Value = action });

                    int rowsAffected = command.ExecuteNonQuery();

                    if (action == 1) // Nếu thêm mới, lấy ID
                    {
                        category.ID = (int)idParameter.Value;
                    }

                    return rowsAffected; // Trả về số dòng bị ảnh hưởng
                }
            }
        }
    }
}
