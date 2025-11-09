using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Lớp ánh xạ bảng Bills
    /// </summary>
    public class Bills
    {
        // ID của hóa đơn, tự tăng trong CSDL
        public int ID { get; set; }

        // Tên hóa đơn
        public string Name { get; set; }

        // ID của bàn liên quan
        public int TableID { get; set; }

        // Tổng tiền của hóa đơn
        public int Amount { get; set; }

        // Chiết khấu (nếu có)
        public float Discount { get; set; }

        // Thuế áp dụng
        public float Tax { get; set; }

        // Trạng thái (0: chưa thanh toán, 1: đã thanh toán)
        public bool Status { get; set; }

        // Thời gian thanh toán
        public DateTime CheckoutDate { get; set; }

        // Tài khoản người dùng thực hiện thanh toán
        public string Account { get; set; }

        // Ngày tạo hóa đơn
        public DateTime Date { get; set; }

        // Tổng giá trị hóa đơn
        public decimal Total { get; set; }
    }
}
