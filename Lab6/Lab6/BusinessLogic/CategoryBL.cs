using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    // Lớp CategoryBL có các phương thức xử lý bảng Category
    public class CategoryBL
    {
        // Đối tượng CategoryDA từ DataAccess
        private CategoryDA categoryDA = new CategoryDA();

        /// <summary>
        /// Lấy tất cả danh mục.
        /// </summary>
        /// <returns>Danh sách các đối tượng Category</returns>
        public List<Category> GetAll()
        {
            return categoryDA.GetAll();
        }

        /// <summary>
        /// Thêm một danh mục mới.
        /// </summary>
        /// <param name="category">Đối tượng Category</param>
        /// <returns>ID của danh mục vừa thêm</returns>
        public int Insert(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                throw new ArgumentException("Loại món ăn không được để trống.");
            }

            return categoryDA.InsertUpdateDelete(category, 1); // 1 = Insert
        }

        /// <summary>
        /// Cập nhật thông tin danh mục.
        /// </summary>
        /// <param name="category">Đối tượng Category</param>
        /// <returns>Số hàng bị ảnh hưởng</returns>
        public int Update(Category category)
        {
            if (category.ID <= 0)
            {
                throw new ArgumentException("ID không hợp lệ.");
            }

            return categoryDA.InsertUpdateDelete(category, 2); // 2 = Update
        }

        /// <summary>
        /// Xóa danh mục theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa</param>
        /// <returns>Số hàng bị ảnh hưởng</returns>
        public int Delete(int categoryId)
        {
            Category category = new Category
            {
                ID = categoryId,
                Name = "", // Giá trị mặc định cho @Name
                Type = 0   // Giá trị mặc định cho @Type
            };
            return new CategoryDA().InsertUpdateDelete(category, 3); // 3 là action cho xóa
        }
    }
}
