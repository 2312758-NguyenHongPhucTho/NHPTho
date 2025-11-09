using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Basic_Command
{
    public partial class BillDetailsForm : Form
    {
        private string _billID;
        private const string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
        public BillDetailsForm(string billID)
        {
            InitializeComponent();
            this._billID = billID; // Lưu BillID vào biến cục bộ
            this.Text = "Chi tiết Hóa đơn ID: " + billID;
        }

        private void lvBillDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        private void LoadBillDetailsData()
        {
            // Giả định:
            // - ListView trên Form này có tên là lvBillDetails
            // - Label/TextBox hiển thị tổng tiền có tên là lblTongTien (hoặc txtTongTien)

            decimal tongTien = 0;

            if (string.IsNullOrEmpty(_billID))
            {
                MessageBox.Show("Không tìm thấy ID hóa đơn để tải chi tiết.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // ⭐ TRUY VẤN SQL ĐÃ ĐƯỢC SỬA: SỬ DỤNG TÊN BẢNG BillDetails VÀ CỘT Quantity, InvoiceID ⭐
                    string query = @"
                SELECT 
                    F.Name AS FoodName, 
                    BD.Quantity,       -- Đã sửa từ 'Count' sang 'Quantity'
                    F.Price,
                    (BD.Quantity * F.Price) AS Total
                FROM 
                    BillDetails BD     -- Đã sửa từ 'BillDetail' sang 'BillDetails'
                JOIN 
                    Food F ON BD.FoodID = F.ID 
                WHERE 
                    BD.InvoiceID = @InvoiceID"; // Đã sửa từ 'BillID' sang 'InvoiceID'

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    // Gán tham số: @InvoiceID sẽ lấy giá trị từ _billID đã truyền vào Form
                    adapter.SelectCommand.Parameters.AddWithValue("@InvoiceID", _billID);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    lvBillDetails.Items.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        // Lấy giá trị từ các cột
                        string foodName = row["FoodName"].ToString();
                        // ⭐ Đã sửa: Lấy từ cột 'Quantity' ⭐
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        decimal price = Convert.ToDecimal(row["Price"]);
                        decimal total = Convert.ToDecimal(row["Total"]);

                        // Thêm vào ListView (Món ăn, Số lượng, Giá, Thành tiền)
                        ListViewItem item = new ListViewItem(foodName);
                        item.SubItems.Add(quantity.ToString());
                        item.SubItems.Add(price.ToString("N0"));
                        item.SubItems.Add(total.ToString("N0"));

                        lvBillDetails.Items.Add(item);

                        tongTien += total;
                    }

                    // Giả sử Label hiển thị tổng tiền có tên là lblTongTien
                    lblTongTien.Text = tongTien.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xử lý nút click (giữ nguyên)
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadBillDetailsData();
        }
        private void btnTai_Click(object sender, EventArgs e)
        {
            LoadBillDetailsData();
        }

        private void BillDetailsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
