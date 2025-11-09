using DataAccess;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BillsBL
    {
        // Đối tượng BillsDA từ DataAccess
        BillsDA billsDA = new BillsDA();
        BillDetailsDA billDetailsDA = new BillDetailsDA();

        // Phương thức lấy hết dữ liệu
        public List<Bills> GetAll()
        {
            return billsDA.GetAll();
        }

        // Phương thức thêm dữ liệu
        public int Insert(Bills bills)
        {
            return billsDA.InsertUpdateDelete(bills, 0); // Action = 0: Insert
        }

        // Phương thức cập nhật dữ liệu
        public int Update(Bills bills)
        {
            return billsDA.InsertUpdateDelete(bills, 1); // Action = 1: Update
        }

        // Phương thức xóa dữ liệu
        public int Delete(Bills bills)
        {
            return billsDA.InsertUpdateDelete(bills, 2); // Action = 2: Delete
        }

        // Phương thức trả về danh sách chi tiết hóa đơn
        public List<BillDetails> GetBillDetails(int billId)
        {
            return billDetailsDA.GetBillDetails(billId);
        }

        // Phương thức xử lý thanh toán hóa đơn
        public bool PayBill(int billId, decimal discount, decimal finalTotal)
        {
            return billsDA.PayBill(billId, discount, finalTotal);
        }

        public DataTable GetRevenueReport(DateTime fromDate, DateTime toDate)
        {
            DataTable reportTable = new DataTable();

            string query = "SELECT CONVERT(DATE, CheckoutDate) AS ReportDate, COUNT(ID) AS InvoiceCount, SUM(Total) AS TotalAmount " +
                           "FROM Bills " +
                           "WHERE Status=1 AND CheckoutDate BETWEEN @FromDate AND @ToDate " +
                           "GROUP BY CONVERT(DATE, CheckoutDate)";

            using (SqlConnection conn = new SqlConnection(Ultilities.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(reportTable);
            }

            return reportTable;
        }
    }
}
