using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AccountBL
    {
        // Đối tượng AccountDA từ DataAccess
        AccountDA accountDA = new AccountDA();

        // Phương thức lấy hết dữ liệu
        public List<Account> GetAll()
        {
            return accountDA.GetAll();
        }

        // Phương thức thêm dữ liệu
        public int Insert(Account account)
        {
            return accountDA.InsertUpdateDelete(account, 0); // Action = 0: Insert
        }

        // Phương thức cập nhật dữ liệu
        public int Update(Account account)
        {
            return accountDA.InsertUpdateDelete(account, 1); // Action = 1: Update
        }

        // Phương thức xóa dữ liệu
        public int Delete(Account account)
        {
            return accountDA.InsertUpdateDelete(account, 2); // Action = 2: Delete
        }

        // Phương thức kiểm tra tồn tại
        public bool CheckAccountExists(string accountName)
        {
            AccountDA accountDA = new AccountDA();
            return accountDA.CheckAccountExists(accountName);
        }

    }
}
