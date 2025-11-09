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
    public partial class BillsForm : Form
    {
        private SqlConnection conn;
        private SqlDataAdapter da;
        private DataTable dtBills;
        string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

        public BillsForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        private void btnLoadBill_Click(object sender, EventArgs e)
        {
            LoadBillDataToListView();
        }
        private void LoadBillDataToListView()
        {
            // Giả định:
            // 1. ListView có tên là lvBillList
            // 2. Tên bảng Hóa đơn trong SQL là 'Bill' (hoặc tên chính xác của bạn)
            // 3. Tên các Textbox/Label tính tổng là txtTongTruocGiam, txtTongGiam, txtThucThu

            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            // Khai báo biến tính tổng
            decimal tongTruocGiam = 0;
            decimal tongGiam = 0;
            decimal thucThu = 0;

            // Lấy ngày tháng từ DateTimePicker (Giả sử tên là dtpFrom và dtpTo)
            // DateTime fromDate = dtpFrom.Value;
            // DateTime toDate = dtpTo.Value.AddDays(1).AddSeconds(-1); // Đảm bảo lấy đến cuối ngày To

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // ⭐ TRUY VẤN SQL ⭐
                    string query = @"SELECT 
                                ID, 
                                Name, 
                                TableID, 
                                Amount, 
                                Discount, 
                                Tax, 
                                Status, 
                                CheckoutDate, 
                                Account
                             FROM Bills
                             -- Nếu lọc theo ngày: WHERE CheckoutDate >= @FromDate AND CheckoutDate <= @ToDate
                             ORDER BY CheckoutDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    // Nếu dùng lọc ngày:
                    // adapter.SelectCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    // adapter.SelectCommand.Parameters.AddWithValue("@ToDate", toDate);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // ⭐ XÓA DỮ LIỆU CŨ TRONG LISTVIEW ⭐
                    lvBills.Items.Clear();

                    // ⭐ 1. LẶP VÀ GÁN DỮ LIỆU TỪ DATATABLE VÀO LISTVIEW ⭐
                    foreach (DataRow row in dt.Rows)
                    {
                        ListViewItem item = new ListViewItem(row["ID"].ToString()); // SubItem 0: ID (Item chính)

                        // SubItem 1: Tên hóa đơn
                        item.SubItems.Add(row["Name"].ToString());
                        // SubItem 2: Bàn
                        item.SubItems.Add(row["TableID"].ToString());

                        // Lấy các giá trị tiền tệ để tính tổng và hiển thị (Dùng ToString("N0") để định dạng)
                        decimal thanhTien = Convert.ToDecimal(row["Amount"] ?? 0);
                        decimal giamGia = Convert.ToDecimal(row["Discount"] ?? 0);
                        decimal taxValue = Convert.ToDecimal(row["Tax"] ?? 0);
                        decimal thucTe = thanhTien + taxValue - giamGia;

                        // SubItem 3: Thành tiền (TotalAmount)
                        item.SubItems.Add(thanhTien.ToString("N0"));
                        // SubItem 4: Giảm giá (Discount)
                        item.SubItems.Add(giamGia.ToString("N0"));
                        // SubItem 5: Thuế (Tax)
                        item.SubItems.Add(taxValue.ToString("N0"));

                        // SubItem 6: Trạng thái
                        item.SubItems.Add(row["Status"].ToString());
                        // SubItem 7: Ngày thanh toán
                        item.SubItems.Add(row["CheckoutDate"].ToString());
                        // SubItem 8: Tài khoản
                        item.SubItems.Add(row["Account"].ToString());

                        // Thêm Item vào ListView
                        lvBills.Items.Add(item);

                        // ⭐ 2. Tính toán tổng cộng ⭐
                        tongTruocGiam += thanhTien;
                        tongGiam += giamGia;
                        thucThu += thucTe;
                    }

                    // ⭐ 3. Hiển thị kết quả tính tổng (Có thể làm tròn) ⭐
                    txtTongTruocGiam.Text = tongTruocGiam.ToString("N0");
                    txtTongGiam.Text = tongGiam.ToString("N0");
                    txtThucThu.Text = thucThu.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Hóa đơn vào ListView: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
      
    

        private void lvBills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Đảm bảo có mục nào đó được chọn
            if (lvBills.SelectedItems.Count > 0)
            {
                // Lấy mục được chọn (là Item chính - SubItem 0)
                ListViewItem selectedItem = lvBills.SelectedItems[0];

                // Lấy ID hóa đơn (Giả sử ID là SubItem 0)
                string billID = selectedItem.SubItems[0].Text;

                // Kiểm tra ID
                if (string.IsNullOrEmpty(billID))
                {
                    MessageBox.Show("Không thể lấy ID hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ⭐ MỞ FORM CHI TIẾT VÀ TRUYỀN ID ⭐

                // Giả sử tên Form chi tiết là BillDetailsForm
                BillDetailsForm detailForm = new BillDetailsForm(billID);
                detailForm.ShowDialog();
            }
        }

        private void BillsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
