using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lab_Advanced_Command
{
    public partial class OrderDetailsForm : Form
    {
        private int billID;
        private readonly string _connString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
       

        public OrderDetailsForm(int billID)
        {
            InitializeComponent();
            this.billID = billID;
        }

        private void OrderDetailsForm_Load(object sender, EventArgs e)
        {
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string query = @"
            SELECT f.Name AS [Tên món ăn],
                   f.Unit AS [Đơn vị],
                   bd.Quantity AS [Số lượng],
                   f.Price AS [Đơn giá],
                   (bd.Quantity * f.Price) AS [Thành tiền]
            FROM BillDetails bd
            JOIN Food f ON bd.FoodID = f.ID
            WHERE bd.InvoiceID = @billID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@billID", billID);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                conn.Open();
                adapter.Fill(dt);
                conn.Close();

                dgvOrderDetails.DataSource = dt;

                // 👉 Tính tổng tiền
                decimal tongTien = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Thành tiền"] != DBNull.Value)
                        tongTien += Convert.ToDecimal(row["Thành tiền"]);
                }

                // 👉 Hiển thị ra label
                lblTongTien.Text = "Tổng tiền: " + tongTien.ToString("N0") + " VND";
            }
        }

        private void dgvOrderDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
