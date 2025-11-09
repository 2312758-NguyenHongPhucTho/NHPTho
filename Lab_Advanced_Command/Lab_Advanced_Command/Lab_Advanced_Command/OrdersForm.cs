using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lab_Advanced_Command
{
    public partial class OrdersForm : Form
    {
        private readonly string _connString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
     
        public OrdersForm()
        {
            InitializeComponent();
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            // Mặc định chọn ngày hôm nay
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1); // đến hết ngày

                // ⚡ Cho phép hóa đơn có CheckoutDate NULL vẫn hiển thị
                string query = @"
                    SELECT ID, Name, TableID, Amount, Discount,Tax, Status, CheckoutDate,Account
                    FROM Bills
                    WHERE (CheckoutDate BETWEEN @from AND @to)
                          OR CheckoutDate IS NULL";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", fromDate);
                cmd.Parameters.AddWithValue("@to", toDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();

                dgvBills.DataSource = dt;

                // ⚡ Tính tổng tiền
                decimal tongTien = 0, giamGia = 0, thucThu = 0;

                foreach (DataRow row in dt.Rows)
                {
                    // Kiểm tra giá trị có null không
                    decimal amount = row["Amount"] != DBNull.Value ? Convert.ToDecimal(row["Amount"]) : 0;
                    decimal discount = row["Discount"] != DBNull.Value ? Convert.ToDecimal(row["Discount"]) : 0;

                    tongTien += amount;
                    giamGia += amount * discount;
                }

                thucThu = tongTien - giamGia;

                lblTongTien.Text = $"{tongTien:N0} VND";
                lblGiamGia.Text = $"{giamGia:N0} VND";
                lblThucThu.Text = $"{thucThu:N0} VND";
            }
        }

        private void dgvBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu người dùng click vào header hoặc dòng trống thì bỏ qua
            if (e.RowIndex < 0) return;

            // Lấy ID hóa đơn từ cột "ID"
            object idValue = dgvBills.Rows[e.RowIndex].Cells["ID"].Value;
            if (idValue == null || idValue == DBNull.Value) return;

            int billID = Convert.ToInt32(idValue);

            // Mở form chi tiết
            OrderDetailsForm detailsForm = new OrderDetailsForm(billID);
            detailsForm.ShowDialog();
        }

        private void dgvBills_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu người dùng click vào header hoặc dòng trống thì bỏ qua
            if (e.RowIndex < 0) return;

            // Lấy ID hóa đơn từ cột "ID"
            object idValue = dgvBills.Rows[e.RowIndex].Cells["ID"].Value;
            if (idValue == null || idValue == DBNull.Value) return;

            int billID = Convert.ToInt32(idValue);

            // Mở form chi tiết
            OrderDetailsForm detailsForm = new OrderDetailsForm(billID);
            detailsForm.ShowDialog();
        }
    }
}
