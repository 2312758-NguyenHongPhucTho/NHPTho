using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BillDetailsBL
    {
        // Đối tượng BillDetailsDA từ DataAccess
        BillDetailsDA billDetailsDA = new BillDetailsDA();

        // Phương thức lấy hết dữ liệu
        public List<BillDetails> GetAll()
        {
            return billDetailsDA.GetAll();
        }

        // Phương thức thêm dữ liệu
        public int Insert(BillDetails billDetails)
        {
            return billDetailsDA.InsertUpdateDelete(billDetails, 0); // Action = 0: Insert
        }

        // Phương thức cập nhật dữ liệu
        public int Update(BillDetails billDetails)
        {
            return billDetailsDA.InsertUpdateDelete(billDetails, 1); // Action = 1: Update
        }

        // Phương thức xóa dữ liệu
        public int Delete(BillDetails billDetails)
        {
            return billDetailsDA.InsertUpdateDelete(billDetails, 2); // Action = 2: Delete
        }
    }
}
