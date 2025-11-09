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
    public partial class AccountManagerForm : Form
    {
        // 🔹 Chuỗi kết nối (chỉnh lại cho đúng SQL Server của bạn)
        string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
        public AccountManagerForm()
        {
            InitializeComponent();
        }


        private bool IsRoleAccountExists(int roleID, string accountName, string connectionString)
        {
            // Kiểm tra xem sự kết hợp của RoleID và AccountName đã tồn tại chưa
            string query = "SELECT COUNT(*) FROM RoleAccount WHERE RoleID = @RoleID AND AccountName = @AccountName";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RoleID", roleID);
                    command.Parameters.AddWithValue("@AccountName", accountName);
                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0; // Trả về true nếu đã tồn tại
                    }
                    catch (Exception)
                    {
                        // Xử lý lỗi kết nối
                        return true;
                    }
                }
            }
        }
       

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string accountName = txtUserName.Text.Trim();
            string note = txtNote.Text.Trim();
            bool actived = checkActive.Checked;

            // Lấy RoleID người dùng nhập
            int roleID;
            if (!int.TryParse(txtRoleID.Text.Trim(), out roleID))
            {
                MessageBox.Show("Vui lòng nhập RoleID là số nguyên hợp lệ!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleID.Focus();
                return;
            }

            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            if (string.IsNullOrEmpty(accountName))
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }

            // Kiểm tra trùng tài khoản + RoleID
            if (IsRoleAccountExists(roleID, accountName, connectionString))
            {
                MessageBox.Show($"Tài khoản '{accountName}' đã tồn tại trong quyền ID {roleID}.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra RoleID có tồn tại trong bảng Role không
                    string checkRole = "SELECT COUNT(*) FROM Role WHERE ID = @RoleID";
                    using (SqlCommand checkCmd = new SqlCommand(checkRole, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@RoleID", roleID);
                        int exists = (int)checkCmd.ExecuteScalar();
                        if (exists == 0)
                        {
                            MessageBox.Show("RoleID không tồn tại trong bảng Role!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Thực hiện thêm dữ liệu
                    string query = @"INSERT INTO RoleAccount (RoleID, AccountName, Actived, Notes)
                             VALUES (@RoleID, @AccountName, @Actived, @Notes)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoleID", roleID);
                        cmd.Parameters.AddWithValue("@AccountName", accountName);
                        cmd.Parameters.AddWithValue("@Actived", actived);
                        cmd.Parameters.AddWithValue("@Notes", note);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm tài khoản vào RoleAccount thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Làm mới DataGridView
                            LoadRoleAccountData();

                            // Xóa nội dung sau khi thêm
                            txtUserName.Clear();
                            txtRoleID.Clear();
                            txtNote.Clear();
                            checkActive.Checked = false;
                            txtUserName.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm tài khoản!", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



  
        

        // (Tùy chọn) Tự động load khi mở form
    

        private void button2_Click(object sender, EventArgs e)
        {
            LoadRoleAccountData();
        }
        private void LoadRoleAccountData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Giữ nguyên dữ liệu theo bảng RoleAccount
                    string query = "SELECT RoleID, AccountName, Actived, Notes FROM RoleAccount";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Gán dữ liệu vào DataGridView
                    dgvRoleAccount.DataSource = dt;

                    // Giữ nguyên tiêu đề gốc trong SQL (không đổi HeaderText)
                    dgvRoleAccount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvRoleAccount.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu RoleAccount: " + ex.Message);
            }
        }

        // (Tùy chọn) Load khi form mở
        private void frmRoleAccount_Load(object sender, EventArgs e)
        {
            LoadRoleAccountData();
        }
    
     

        private void chkActiveOnly_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // 🔸 Kết nối tới SQL Server
                

                // 🔸 Câu lệnh SQL lấy dữ liệu
                string query = "SELECT AccountName, Password, FullName, Email, Tell, DateCreated FROM Account";

                // 🔸 Tạo đối tượng kết nối và lấy dữ liệu
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // 🔸 Gán dữ liệu vào DataGridView
                    dgvAccount.DataSource = dt;
                }

                // 🔸 (Tuỳ chọn) chỉ cho phép xem
                dgvAccount.ReadOnly = true;
                dgvAccount.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo click vào dòng hợp lệ
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAccount.Rows[e.RowIndex];

                txtName.Text = row.Cells[0].Value?.ToString();
                txtPassword.Text = row.Cells[1].Value?.ToString();
                txtFullName.Text = row.Cells[2].Value?.ToString();
                txtEmail.Text = row.Cells[3].Value?.ToString();
                txtTell.Text = row.Cells[4].Value?.ToString();
                txtDate.Text = row.Cells[5].Value?.ToString();
            }



        }
        private void LoadDataToDataGridView()
        {
            // Chuỗi kết nối và truy vấn SELECT
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            string query = "SELECT AccountName, Password, FullName, Email, Tell, DateCreated FROM Account";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Gán DataSource lại cho DataGridView của bạn (có vẻ tên là dgvAccount)
                    dgvAccount.DataSource = dt;

                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                }
            }
        }
        private void ThemTaiKhoan_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ các controls (Giữ nguyên)
            string accountName = txtName.Text;
            string password = txtPassword.Text;
            string fullName = txtFullName.Text;
            string email = txtEmail.Text;
            string tell = txtTell.Text;
            string dateCreated = txtDate.Text;

            // Kiểm tra dữ liệu bắt buộc (Giữ nguyên)
            if (string.IsNullOrWhiteSpace(accountName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Tên tài khoản và Mật khẩu không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- Bắt đầu quá trình Thêm vào SQL Server (Giữ nguyên) ---
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            string query = "INSERT INTO Account (AccountName, Password, FullName, Email, Tell, DateCreated) " +
                           "VALUES (@AccountName, @Password, @FullName, @Email, @Tell, @DateCreated)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm các tham số (Giữ nguyên)
                    command.Parameters.AddWithValue("@AccountName", accountName);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Tell", tell);

                    DateTime createdDate;
                    if (DateTime.TryParse(dateCreated, out createdDate))
                    {
                        command.Parameters.AddWithValue("@DateCreated", createdDate);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@DateCreated", DBNull.Value);
                    }

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm tài khoản thành công vào cơ sở dữ liệu.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // ******************************************************
                            // ⭐ BƯỚC CẬP NHẬT DATAGRIDVIEW VÀ XÓA INPUT ⭐
                            // 2. Tải lại dữ liệu (Refresh DataGridView)
                            LoadDataToDataGridView();

                            // 3. Xóa nội dung trên các controls
                            ClearInputControls();
                            // ******************************************************
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm tài khoản vào cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi kết nối hoặc thực thi SQL: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ClearInputControls()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtTell.Clear();
            txtDate.Clear();
            // Có thể set focus lại cho control đầu tiên
            txtName.Focus();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DÒNG ĐANG CHỌN TỪ DATAGRIDVIEW ⭐

            // Ép kiểu để lấy DataGridView (dgvRoleAccount) đã mở ContextMenuStrip
            ContextMenuStrip cms = (ContextMenuStrip)((ToolStripMenuItem)sender).Owner;
            DataGridView dgvRoleAccount = cms.SourceControl as DataGridView;

            if (dgvRoleAccount == null || dgvRoleAccount.CurrentRow == null || dgvRoleAccount.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần vô hiệu hóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgvRoleAccount.CurrentRow;

            // ⭐ 2. LẤY DỮ LIỆU KEY (RoleID và AccountName) ⭐
            // Giả định: RoleID ở Index 0, AccountName ở Index 1

            if (selectedRow.Cells[0].Value == null || selectedRow.Cells[1].Value == null)
            {
                MessageBox.Show("Dữ liệu khóa (RoleID/AccountName) không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int roleID = Convert.ToInt32(selectedRow.Cells[0].Value);
            string accountName = selectedRow.Cells[1].Value.ToString();

            // ⭐ 3. XÁC NHẬN HÀNH ĐỘNG ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn VÔ HIỆU HÓA (chuyển Actived về FALSE) tài khoản '{accountName}' (Role ID: {roleID}) không?",
                "Xác nhận Vô Hiệu Hóa Tài Khoản",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 4. THỰC HIỆN UPDATE SQL ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh UPDATE: Set Actived = False (hoặc 0)
                    string query = @"UPDATE RoleAccount 
                             SET Actived = @Actived 
                             WHERE RoleID = @RoleID AND AccountName = @AccountName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Actived", false); // Chuyển Actived thành False
                    cmd.Parameters.AddWithValue("@RoleID", roleID);
                    cmd.Parameters.AddWithValue("@AccountName", accountName);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    // ⭐ 5. THÔNG BÁO VÀ REFRESH ⭐
                    if (result > 0)
                    {
                        MessageBox.Show($"Đã vô hiệu hóa (Actived = FALSE) tài khoản '{accountName}'.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Gọi hàm load dữ liệu để làm mới DataGridView
                        LoadRoleAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để cập nhật hoặc trạng thái đã là FALSE.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi vô hiệu hóa tài khoản: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CT_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tool2_Click(object sender, EventArgs e)
        {

            RoleListForm roleListForm = new RoleListForm(); // Tạo form mới
            roleListForm.Show(); // Hiển thị form mới, KHÔNG ẩn form hiện tại
                                 // ⭐ 1. XÁC ĐỊNH DATAGRIDVIEW VÀ DÒNG ĐANG CHỌN ⭐
                                 // 1. LẤY DATAGRIDVIEW VÀ ACCOUNTNAME TỪ DÒNG ĐANG CHỌN

           
            }

    

        private void dgvRoleAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo click vào dòng dữ liệu hợp lệ (không phải Header)
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại được chọn. Dùng sender để code linh hoạt hơn.
                DataGridViewRow row = ((DataGridView)sender).Rows[e.RowIndex];

                // ⭐ GÁN DỮ LIỆU TỪ CÁC CỘT LÊN CONTROLS ⭐
                // Giả định thứ tự cột là: RoleID=0, AccountName=1, Actived=2, Notes=3

                // Cột 1: AccountName (Username)
                // Giả sử TextBox là txtUserName
                txtUserName.Text = row.Cells[1].Value?.ToString();

                // Cột 0: RoleID
                // Giả sử TextBox là txtRoleID
                txtRoleID.Text = row.Cells[0].Value?.ToString();

                // Cột 3: Notes
                // Giả sử TextBox là txtNote
                txtNote.Text = row.Cells[3].Value?.ToString();

                // Cột 2: Actived (Xử lý cho CheckBox)
                bool activedValue = false;
                if (row.Cells[2].Value != null)
                {
                    // Chuyển đổi giá trị True/False từ cơ sở dữ liệu sang boolean
                    Boolean.TryParse(row.Cells[2].Value.ToString(), out activedValue);
                }
                // Giả sử CheckBox có tên là chkActive
                checkActive.Checked = activedValue;
            }
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // ⭐ 1. KHAI BÁO DỮ LIỆU ĐẦU VÀO TỪ TEXTBOXES ⭐
            string accountName = txtName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string tell = txtTell.Text.Trim();
            string dateCreated = txtDate.Text.Trim(); // Lấy giá trị DateCreated

            // Kiểm tra dữ liệu bắt buộc (AccountName là khóa chính)
            if (string.IsNullOrEmpty(accountName))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần cập nhật hoặc nhập Account Name.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. HỎI XÁC NHẬN TỪ NGƯỜI DÙNG ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn cập nhật thông tin cho tài khoản '{accountName}' không?",
                "Xác nhận Cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 3. THỰC HIỆN UPDATE SQL ⭐
            // Thay đổi chuỗi kết nối của bạn nếu cần
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh UPDATE: Cập nhật tất cả các trường dựa trên AccountName
                    string query = @"UPDATE Account 
                             SET Password = @Password, 
                                 FullName = @FullName, 
                                 Email = @Email, 
                                 Tell = @Tell,
                                 DateCreated = @DateCreated 
                             WHERE AccountName = @AccountName";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Gán giá trị vào Parameters
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Xử lý giá trị có thể NULL cho cột Tell và DateCreated
                    if (string.IsNullOrEmpty(tell))
                        cmd.Parameters.AddWithValue("@Tell", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Tell", tell);

                    if (string.IsNullOrEmpty(dateCreated))
                        cmd.Parameters.AddWithValue("@DateCreated", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@DateCreated", dateCreated);

                    // Khóa chính (điều kiện WHERE)
                    cmd.Parameters.AddWithValue("@AccountName", accountName);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    // ⭐ 4. THÔNG BÁO VÀ REFRESH DỮ LIỆU ⭐
                    if (result > 0)
                    {
                        MessageBox.Show($"Cập nhật tài khoản '{accountName}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // 1. Refresh DataGridView Account
                        LoadDataToDataGridView();
                        // 2. Xóa nội dung trên các controls
                        ClearInputControls();
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy tài khoản '{accountName}' để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearRoleAccountInputControls()
        {
            txtUserName.Text = "";
            txtRoleID.Text = "";
            txtNote.Text = "";
            checkActive.Checked = false;
        }
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DỮ LIỆU TỪ CONTROLS DƯỚI ⭐
            // Giả định tên controls là: txtUserName, txtRoleID, txtNote, chkActive
            string userName = txtUserName.Text.Trim();
            string roleIDText = txtRoleID.Text.Trim();
            string notes = txtNote.Text.Trim();
            bool actived = checkActive.Checked;

            // Kiểm tra các trường bắt buộc (Username và RoleID là khóa chính kép)
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(roleIDText))
            {
                MessageBox.Show("Username và RoleID không được để trống khi cập nhật.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(roleIDText, out int roleID))
            {
                MessageBox.Show("RoleID phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. HỎI XÁC NHẬN TỪ NGƯỜI DÙNG ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn cập nhật quyền cho tài khoản '{userName}' với RoleID '{roleID}' không?",
                "Xác nhận Cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 3. THỰC HIỆN UPDATE SQL TRÊN BẢNG RoleAccount ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh UPDATE RoleAccount (chỉ cập nhật Actived và Notes)
                    string query = @"UPDATE RoleAccount 
                             SET Actived = @Actived, 
                                 Notes = @Notes 
                             WHERE AccountName = @AccountName AND RoleID = @RoleID";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Gán giá trị cập nhật
                    cmd.Parameters.AddWithValue("@Actived", actived);
                    if (string.IsNullOrEmpty(notes))
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Notes", notes);

                    // Điều kiện WHERE (Khóa chính)
                    cmd.Parameters.AddWithValue("@AccountName", userName);
                    cmd.Parameters.AddWithValue("@RoleID", roleID);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    // ⭐ 4. THÔNG BÁO VÀ REFRESH DỮ LIỆU ⭐
                    if (result > 0)
                    {
                        MessageBox.Show($"Cập nhật quyền thành công cho tài khoản '{userName}'.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // LoadRoleAccountData() là hàm tải lại DataGridView RoleAccount (lưới dưới)
                        LoadRoleAccountData();
                    }
                    else
                    {
                        // Bạn đã gặp lỗi này trước đó
                        MessageBox.Show($"Không tìm thấy bản ghi RoleAccount cần cập nhật. Vui lòng kiểm tra lại Username và RoleID.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật RoleAccount: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AccountManagerForm_Load(object sender, EventArgs e)
        {

        }
    }
    }
    

