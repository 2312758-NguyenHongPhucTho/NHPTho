using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
namespace RestaurantManagementProject
{
    public partial class frmBills : Form
    {
        List<Bills> listBills = new List<Bills>();
        // Đối tượng Bills đang chọn hiện hành
        Bills billCurrent = new Bills();
        public frmBills()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtTableID.Text = "";
            txtAmount.Text = "0";
            txtDiscount.Text = "0";
            txtTax.Text = "0";
            txtAccount.Text = "";
            cbbStatus.SelectedIndex = 0; // Mặc định là "Chưa thanh toán"
            dtpCheckoutDate.Value = DateTime.Now;
        }
        private void LoadBillsDataToListView()
        {
            // Khởi tạo đối tượng BillsBL từ tầng BusinessLogic
            BillsBL billsBL = new BillsBL();

            // Lấy danh sách hóa đơn từ tầng BusinessLogic
            listBills = billsBL.GetAll();

            // Sắp xếp danh sách hóa đơn theo một tiêu chí (ví dụ: Tên hóa đơn)
            listBills = listBills.OrderBy(bill => bill.Name).ToList();

            // Biến số thứ tự cho từng dòng
            int count = 1;

            // Xóa dữ liệu cũ trong ListView
            lsvBills.Items.Clear();

            // Duyệt danh sách dữ liệu để hiển thị trong ListView
            foreach (var bill in listBills)
            {
                // Kiểm tra nếu dữ liệu hóa đơn là null
                if (bill == null)
                {
                    continue;
                }

                // Tạo một dòng trong ListView, bắt đầu bằng số thứ tự
                ListViewItem item = new ListViewItem(count.ToString());

                // Thêm các cột dữ liệu
                item.SubItems.Add(bill.Name ?? "N/A"); // Tên hóa đơn
                item.SubItems.Add(bill.TableID.ToString()); // Mã bàn
                item.SubItems.Add(bill.Amount.ToString("N0")); // Tổng tiền
                item.SubItems.Add(bill.Discount.ToString("P0")); // Chiết khấu
                item.SubItems.Add(bill.Status ? "Đã thanh toán" : "Chưa thanh toán"); // Trạng thái
                item.SubItems.Add(bill.Tax.ToString("P0")); // Thuế
                item.SubItems.Add(bill.CheckoutDate.ToString("dd/MM/yyyy")); // Ngày thanh toán
                item.SubItems.Add(bill.Account ?? "N/A"); // Người thanh toán

                // Thêm dòng vào ListView
                lsvBills.Items.Add(item);

                // Tăng số thứ tự
                count++;
            }
        }

        private void frmBills_Load(object sender, EventArgs e)
        {
            cbbStatus.Items.Clear();
            cbbStatus.Items.Add("Chưa thanh toán");
            cbbStatus.Items.Add("Đã thanh toán");
            cbbStatus.SelectedIndex = 0; // Mặc định là "Chưa thanh toán"

            // Đổ dữ liệu vào ListView
            LoadBillsDataToListView();
        }
        public int InsertBill()
        {
            // Kiểm tra nếu các ô nhập khác rỗng
            if (string.IsNullOrEmpty(txtName?.Text) || string.IsNullOrEmpty(txtTableID?.Text))
            {
                MessageBox.Show("Vui lòng nhập tên hóa đơn và mã bàn.");
                return -1;
            }

            // Khai báo đối tượng Bills từ tầng DataAccess
            Bills bill = new Bills
            {
                Name = txtName.Text,
                Account = txtAccount.Text,
                TableID = int.TryParse(txtTableID.Text, out int tableId) ? tableId : 0,
                Amount = int.TryParse(txtAmount.Text, out int amount) ? amount : 0,
                Discount = float.TryParse(txtDiscount.Text, out float discount) ? discount : 0,
                Tax = float.TryParse(txtTax.Text, out float tax) ? tax : 0,
                CheckoutDate = dtpCheckoutDate.Value,
                Status = cbbStatus.SelectedIndex == 1 // Đã thanh toán nếu index = 1
            };

            BillsBL billsBL = new BillsBL();
            return billsBL.Insert(bill);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi phương thức thêm dữ liệu
                int result = InsertBill();

                if (result > 0)
                {
                    MessageBox.Show("Thêm hóa đơn thành công!");
                    LoadBillsDataToListView();
                }
                else
                {
                    MessageBox.Show("Thêm hóa đơn không thành công. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình thêm dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBillDetails frm = new frmBillDetails();
            frm.ShowDialog();
        }

        private void lsvBills_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (lsvBills.SelectedItems.Count > 0)
            {
                // Lấy ListViewItem đầu tiên được chọn
                ListViewItem selectedItem = lsvBills.SelectedItems[0];

                // Lấy dữ liệu từ các cột (SubItems) và gán vào các control tương ứng

                // **Thứ tự các cột dựa trên hình ảnh:**
                // Cột 0: STT (SubItems[0] hoặc Text) -> Không cần gán vào TextBox
                // Cột 1: Tên hóa đơn (Text)
                // Cột 2: Mã bàn (SubItems[1])
                // Cột 3: Tổng tiền (SubItems[2])
                // Cột 4: Giảm giá (SubItems[3])
                // Cột 5: Trạng thái (SubItems[4])
                // Cột 6: Thuế (SubItems[5])
                // Cột 7: Ngày thanh toán (SubItems[6])

                // 1. Tên hóa đơn (Text chính của ListViewItem)
                txtName.Text = selectedItem.Text;

                // 2. Mã bàn (SubItems[1])
                txtTableID.Text = selectedItem.SubItems[1].Text;

                // 3. Tổng tiền (SubItems[2])
                txtAmount.Text = selectedItem.SubItems[2].Text;

                // 4. Giảm giá (SubItems[3])
                txtDiscount.Text = selectedItem.SubItems[3].Text;

                // 5. Thuế (SubItems[5])
                txtTax.Text = selectedItem.SubItems[5].Text;

                // 6. Trạng thái (SubItems[4]) - Dùng ComboBox hoặc TextBox
                // Nếu là ComboBox (ví dụ: cboTrangThai)
                cbbStatus.Text = selectedItem.SubItems[4].Text;

                txtAccount.Text = selectedItem.SubItems[7].Text;

                // 7. Ngày thanh toán (SubItems[6]) - Dùng DateTimePicker
                string ngayThanhToan = selectedItem.SubItems[6].Text;
                if (DateTime.TryParse(ngayThanhToan, out DateTime dateValue))
                {
                    dtpCheckoutDate.Value = dateValue;
                }

                // 8. Người thanh toán - (Giả định là cột cuối cùng, cần kiểm tra trong ListView của bạn)
                // txtNguoiThanhToan.Text = selectedItem.SubItems[7].Text; // Nếu có thêm cột Người thanh toán
            }
            else
            {
                // Xóa nội dung hoặc đặt lại giá trị mặc định nếu không có dòng nào được chọn
                txtName.Text = "";
                txtTableID.Text = "";
                txtAmount.Text = "";
                txtDiscount.Text = "";
                txtTax.Text = "";
                cbbStatus.SelectedIndex = -1; // Đặt lại ComboBox
                txtAccount.Text = "";
                dtpCheckoutDate.Value = DateTime.Now; // Đặt lại ngày hiện tại
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
    }
