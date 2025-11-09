using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    // Lớp FoodBL có các phương thức xử lý bảng Food
    public class FoodBL
    {
        private FoodDA foodDA = new FoodDA();

        /// <summary>
        /// Lấy tất cả món ăn.
        /// </summary>
        /// <returns>Danh sách các đối tượng Food</returns>
        public List<Food> GetAll()
        {
            return foodDA.GetAll();
        }

        /// <summary>
        /// Lấy món ăn theo ID.
        /// </summary>
        /// <param name="ID">ID của món ăn</param>
        /// <returns>Đối tượng Food (hoặc null nếu không tìm thấy)</returns>
        public Food GetByID(int ID)
        {
            return GetAll().FirstOrDefault(food => food.ID == ID);
        }

        /// <summary>
        /// Tìm kiếm món ăn theo từ khóa.
        /// </summary>
        /// <param name="key">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách các món ăn phù hợp</returns>
        public List<Food> Find(string key)
        {
            List<Food> list = GetAll();
            return list.Where(food =>
                food.ID.ToString().Contains(key) ||
                (!string.IsNullOrEmpty(food.Name) && food.Name.Contains(key)) ||
                (!string.IsNullOrEmpty(food.Unit) && food.Unit.Contains(key)) ||
                food.Price.ToString().Contains(key) ||
                (!string.IsNullOrEmpty(food.Notes) && food.Notes.Contains(key))
            ).ToList();
        }

        /// <summary>
        /// Thêm món ăn mới.
        /// </summary>
        /// <param name="food">Đối tượng Food</param>
        /// <returns>ID của món ăn vừa thêm</returns>
        public int Insert(Food food)
        {
            if (string.IsNullOrEmpty(food.Name))
            {
                throw new ArgumentException("Tên món ăn không được để trống.");
            }
            if (food.FoodCategoryID <= 0)
            {
                throw new ArgumentException("Danh mục không hợp lệ.");
            }
            if (food.Price < 0)
            {
                throw new ArgumentException("Giá không được nhỏ hơn 0.");
            }

            return foodDA.InsertUpdateDelete(food, 1); // 1 = Insert
        }

        /// <summary>
        /// Cập nhật món ăn.
        /// </summary>
        /// <param name="food">Đối tượng Food</param>
        /// <returns>Số hàng bị ảnh hưởng</returns>
        public int Update(Food food)
        {
            if (food.ID <= 0)
            {
                throw new ArgumentException("ID không hợp lệ.");
            }
            return foodDA.InsertUpdateDelete(food, 2); // 2 = Update
        }

        /// <summary>
        /// Xóa món ăn theo ID.
        /// </summary>
        /// <param name="id">ID của món ăn</param>
        /// <returns>Số hàng bị ảnh hưởng</returns>
        public int Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID không hợp lệ.");
            }

            return foodDA.InsertUpdateDelete(new Food { ID = id }, 3); // 3 = Delete
        }

        public Food GetByName(string name)
        {
            return foodDA.GetByName(name);
        }
    }
}
