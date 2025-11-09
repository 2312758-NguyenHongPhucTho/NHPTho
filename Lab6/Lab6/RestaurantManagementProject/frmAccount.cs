using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using DataAccess;
namespace RestaurantManagementProject
{
    public partial class frmAccount : Form
    {
        List<Account> listAccount = new List<Account>();
        // Đối tượng Account đang chọn hiện hành
        Account accountCurrent = new Account();
        public frmAccount()
        {
            InitializeComponent();
        }
        private void LoadAccountDataToListView()
        {
            // Khởi tạo đối tượng AccountBL từ tầng BusinessLogic
            AccountBL accountBL = new AccountBL();

            // Lấy danh sách tài khoản từ tầng BusinessLogic
            listAccount = accountBL.GetAll();

            // Biến số thứ tự cho từng dòng
            int count = 1;

            // Xoá dữ liệu cũ trong ListView
            lsvAccount.Items.Clear();

            // Duyệt danh sách dữ liệu để hiển thị trong ListView
            foreach (var account in listAccount)
            {
                // Kiểm tra nếu dữ liệu tài khoản là null
                if (account == null)
                {
                    continue;
                }

                // Tạo một dòng trong ListView, bắt đầu bằng số thứ tự
                ListViewItem item = new ListViewItem(count.ToString());

                // Thêm các cột dữ liệu
                item.SubItems.Add(account.AccountName ?? "N/A");
                item.SubItems.Add(account.Password ?? "N/A");
                item.SubItems.Add(account.FullName ?? "N/A");
                item.SubItems.Add(account.Email ?? "N/A");
                item.SubItems.Add(account.Tell ?? "N/A");
                item.SubItems.Add(account.DateCreated.ToString("dd/MM/yyyy"));

                // Thêm dòng vào ListView
                lsvAccount.Items.Add(item);

                // Tăng số thứ tự
                count++;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Gán các ô bằng giá trị mặc định
            txtAccountName.Text = "";
            txtPassword.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            LoadAccountDataToListView();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra giá trị của txtAccountName
                if (string.IsNullOrWhiteSpace(txtAccountName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Account từ dữ liệu nhập
                Account account = new Account
                {
                    AccountName = txtAccountName.Text.Trim(),
                    Password = txtPassword.Text.Trim(),
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Tell = txtPhone.Text.Trim(),
                    DateCreated = DateTime.Now
                };

                // Gọi phương thức thêm dữ liệu
                int result = new AccountBL().Insert(account);

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                    LoadAccountDataToListView();
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
                // Kiểm tra tài khoản hiện tại
                if (accountCurrent == null || accountCurrent.AccountName == "")
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi phương thức cập nhật dữ liệu
                int result = UpdateAccount();

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                    LoadAccountDataToListView();
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
        public int UpdateAccount()
        {
            // Kiểm tra nếu đối tượng Account hiện hành có giá trị
            if (accountCurrent == null)
            {
                MessageBox.Show("Không có dữ liệu tài khoản hiện hành để cập nhật.");
                return -1;
            }

            // Gán dữ liệu chỉnh sửa
            accountCurrent.AccountName = txtAccountName?.Text ?? string.Empty;
            accountCurrent.Password = txtPassword?.Text ?? string.Empty;
            accountCurrent.FullName = txtFullName?.Text ?? string.Empty;
            accountCurrent.Email = txtEmail?.Text ?? string.Empty;
            accountCurrent.Tell = txtPhone?.Text ?? string.Empty;

            // Cập nhật dữ liệu qua tầng Business
            return new AccountBL().Update(accountCurrent);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lsvAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvAccount.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lsvAccount.SelectedItems[0];

                // Lấy dữ liệu từ ListView và gán vào các TextBox
                // **Giả định thứ tự cột (không tính STT) là:**
                // Cột 0 (Text): Tên tài khoản
                // Cột 1 (SubItems[1]): Mật khẩu
                // Cột 2 (SubItems[2]): Họ tên
                // Cột 3 (SubItems[3]): Email

                txtAccountName.Text = selectedItem.Text;
                txtPassword.Text = selectedItem.SubItems[1].Text;
                txtFullName.Text = selectedItem.SubItems[2].Text;
                txtEmail.Text = selectedItem.SubItems[3].Text;
                // Thêm các TextBox khác nếu cần (ví dụ: Số điện thoại, Ngày tạo)
                // txtSoDienThoai.Text = selectedItem.SubItems[4].Text;
            }
            else
            {
                // Xóa nội dung TextBox nếu không có dòng nào được chọn
                txtAccountName.Text = "";
                txtPassword.Text = "";
                txtFullName.Text = "";
                txtEmail.Text = "";
            }
        }
    }
}
