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
    public partial class frmBillDetails : Form
    {
        List<BillDetails> listBillDetails = new List<BillDetails>();
        // Danh sách toàn cục bảng Bills
        List<Bills> listBills = new List<Bills>();
        // Danh sách toàn cục bảng Food
        List<Food> listFood = new List<Food>();
        // Đối tượng BillDetails hiện hành
        BillDetails billDetailsCurrent = new BillDetails();
        public frmBillDetails()
        {
            InitializeComponent();
        }

        private void frmBillDetails_Load(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Gán các ô bằng giá trị mặc định
            txtQuantity.Text = "0";
            // Thiết lập index = 0 cho ComboBox
            if (cbbInvoice.Items.Count > 0)
                cbbInvoice.SelectedIndex = 0;
            if (cbbFood.Items.Count > 0)
                cbbFood.SelectedIndex = 0;
        }
        private void LoadBillDetailsToListView()
        {
            // Khởi tạo đối tượng BillDetailsBL từ tầng BusinessLogic
            BillDetailsBL billDetailsBL = new BillDetailsBL();

            // Lấy danh sách chi tiết hóa đơn từ tầng BusinessLogic
            listBillDetails = billDetailsBL.GetAll();

            // Biến số thứ tự cho từng dòng
            int count = 1;

            // Xoá dữ liệu cũ trong ListView
            lsvBillDetails.Items.Clear();

            // Duyệt danh sách dữ liệu để hiển thị trong ListView
            foreach (var detail in listBillDetails)
            {
                // Kiểm tra nếu dữ liệu null
                if (detail == null)
                {
                    continue;
                }

                // Tạo một dòng trong ListView, bắt đầu bằng số thứ tự
                ListViewItem item = new ListViewItem(count.ToString());

                // Thêm các cột dữ liệu
                var bill = listBills.Find(x => x.ID == detail.InvoiceID);
                item.SubItems.Add(bill?.Name ?? "Unknown Invoice");

                var food = listFood.Find(x => x.ID == detail.FoodID);
                item.SubItems.Add(food?.Name ?? "Unknown Food");

                item.SubItems.Add(detail.Quantity.ToString());

                // Thêm dòng vào ListView
                lsvBillDetails.Items.Add(item);

                // Tăng số thứ tự
                count++;
            }
        }
        public int InsertBillDetails()
        {
            // Khai báo đối tượng BillDetails từ tầng DataAccess
            BillDetails detail = new BillDetails();
            detail.ID = 0;

            // Kiểm tra nếu các ô nhập khác rỗng
            if (string.IsNullOrEmpty(txtQuantity?.Text))
            {
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
            }
            else
            {
                // Nhận giá trị từ người dùng nhập vào
                int quantity = 0;
                if (!int.TryParse(txtQuantity?.Text, out quantity))
                {
                    MessageBox.Show("Số lượng không hợp lệ.");
                    return -1;
                }
                detail.Quantity = quantity;

                // Giá trị InvoiceID được lấy từ ComboBox
                detail.InvoiceID = Convert.ToInt32(cbbInvoice.SelectedValue);

                // Giá trị FoodID được lấy từ ComboBox
                detail.FoodID = Convert.ToInt32(cbbFood.SelectedValue);

                // Khai báo đối tượng BillDetailsBL từ tầng Business
                BillDetailsBL billDetailsBL = new BillDetailsBL();
                // Chèn dữ liệu vào bảng
                return billDetailsBL.Insert(detail);
            }
            return -1;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra giá trị của txtQuantity
                if (string.IsNullOrWhiteSpace(txtQuantity.Text))
                {
                    MessageBox.Show("Vui lòng nhập số lượng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng BillDetails từ dữ liệu nhập
                BillDetails detail = new BillDetails
                {
                    InvoiceID = Convert.ToInt32(cbbInvoice.SelectedValue),
                    FoodID = Convert.ToInt32(cbbFood.SelectedValue),
                    Quantity = int.TryParse(txtQuantity.Text.Trim(), out int quantity) ? quantity : 0
                };

                // Gọi phương thức thêm dữ liệu
                int result = new BillDetailsBL().Insert(detail);

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                    LoadBillDetailsToListView();
                }
                else
                {
                    MessageBox.Show("Thêm dữ liệu không thành công. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình thêm dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra chi tiết hiện tại
                if (billDetailsCurrent == null || billDetailsCurrent.ID <= 0)
                {
                    MessageBox.Show("Vui lòng chọn chi tiết để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng BillDetails từ dữ liệu nhập
                BillDetails detail = new BillDetails
                {
                    ID = billDetailsCurrent.ID,
                    InvoiceID = Convert.ToInt32(cbbInvoice.SelectedValue),
                    FoodID = Convert.ToInt32(cbbFood.SelectedValue),
                    Quantity = int.TryParse(txtQuantity.Text.Trim(), out int quantity) ? quantity : 0
                };

                // Gọi phương thức cập nhật dữ liệu
                int result = new BillDetailsBL().Update(detail);

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                    LoadBillDetailsToListView();
                }
                else
                {
                    MessageBox.Show("Cập nhật dữ liệu không thành công. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình cập nhật dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra chi tiết hóa đơn hiện tại
                if (billDetailsCurrent == null || billDetailsCurrent.ID <= 0)
                {
                    MessageBox.Show("Vui lòng chọn chi tiết hóa đơn để xoá.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận từ người dùng
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá chi tiết này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int result = new BillDetailsBL().Delete(billDetailsCurrent);

                    // Kiểm tra kết quả
                    if (result > 0)
                    {
                        MessageBox.Show("Xoá chi tiết thành công!");
                        LoadBillDetailsToListView();
                    }
                    else
                    {
                        MessageBox.Show("Xoá không thành công. Vui lòng thử lại.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong quá trình xoá dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void LoadInvoices()
        {
            // Khởi tạo đối tượng BillsBL từ tầng BusinessLogic
            BillsBL billsBL = new BillsBL();

            // Lấy dữ liệu hóa đơn từ tầng BusinessLogic
            listBills = billsBL.GetAll();

            // Gán dữ liệu vào ComboBox với ValueMember là ID và DisplayMember là Name
            cbbInvoice.DataSource = listBills;
            cbbInvoice.ValueMember = "ID";
            cbbInvoice.DisplayMember = "Name";
        }
        private void LoadFood()
        {
            // Khởi tạo đối tượng FoodBL từ tầng BusinessLogic
            FoodBL foodBL = new FoodBL();

            // Lấy dữ liệu thực phẩm từ tầng BusinessLogic
            listFood = foodBL.GetAll();

            // Gán dữ liệu vào ComboBox với ValueMember là ID và DisplayMember là Name
            cbbFood.DataSource = listFood;
            cbbFood.ValueMember = "ID";
            cbbFood.DisplayMember = "Name";
        }
      
        private void lsvBillDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Đổ dữ liệu vào ComboBox
            LoadInvoices();
            LoadFood();
            // Đổ dữ liệu vào ListView
            LoadBillDetailsToListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadBillDetailsToListView();
        }
    }
}
