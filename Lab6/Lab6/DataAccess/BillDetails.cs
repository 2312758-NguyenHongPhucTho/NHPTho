using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Lớp ánh xạ bảng BillDetails
    /// </summary>
    public class BillDetails
    {
        // ID của chi tiết hóa đơn
        public int ID { get; set; }

        // ID của hóa đơn liên quan
        public int InvoiceID { get; set; }

        // ID của thức ăn hoặc đồ uống
        public int FoodID { get; set; }

        // Tên món ăn
        public string FoodName { get; set; } // Thêm thuộc tính này

        // Số lượng
        public int Quantity { get; set; }

        // Giá
        public decimal Price { get; set; }

        // Tổng giá trị (Price * Quantity)
        public decimal Total { get; set; }
    }
}
