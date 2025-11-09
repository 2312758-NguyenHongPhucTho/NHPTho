using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Lớp ánh xạ bảng Account
    /// </summary>
    public class Account
    {
        // Tên tài khoản
        public string AccountName { get; set; }

        // Mật khẩu
        public string Password { get; set; }

        // Họ tên đầy đủ
        public string FullName { get; set; }

        // Email của tài khoản
        public string Email { get; set; }

        // Số điện thoại
        public string Tell { get; set; }

        // Ngày tạo tài khoản
        public DateTime DateCreated { get; set; }
    }
}
