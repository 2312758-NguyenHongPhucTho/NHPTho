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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTableData();
        }
        private void LoadTableData()
        {
            // Đảm bảo connectionString đã được khai báo
            // (Sử dụng chuỗi kết nối đã được dùng trong các hàm trước)
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // ⭐ Truy vấn SQL: Lấy các cột Mã bàn, Tên bàn, Trạng thái, Sức chứa ⭐
                    // (Giả sử tên bảng là 'Table' hoặc 'Ban' - tôi dùng 'Table')
                    string query = "SELECT ID, Name, Status, Capacity FROM [dbo].[Table]";
                    // Lưu ý: Dùng [dbo].[Table] nếu tên bảng là Table (vì TABLE là từ khóa SQL)

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // ⭐ Gán dữ liệu cho DataGridView ⭐
                    // (Giả định tên DataGridView để hiển thị Bàn là dgvTable)
                    dgvTable.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Bàn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra: Đảm bảo click vào dòng dữ liệu hợp lệ (không phải Header)
            if (e.RowIndex >= 0)
            {
                // Lấy dòng hiện tại được chọn
                DataGridViewRow row = dgvTable.Rows[e.RowIndex];

                // ⭐ GÁN DỮ LIỆU TỪ CÁC CỘT LÊN TEXTBOXES ⭐
                // Giả định thứ tự cột là: Mã bàn=0, Tên bàn=1, Trạng thái=2, Sức chứa=3

                // Cột 0: Mã bàn (ID)
                // Giả sử TextBox là txtMaBan
                txtMaBan.Text = row.Cells[0].Value?.ToString();

                // Cột 1: Tên bàn (Name)
                // Giả sử TextBox là txtTenBan
                txtTenBan.Text = row.Cells[1].Value?.ToString();

                // Cột 2: Trạng thái (Status)
                // Giả sử TextBox là txtTrangThai
                txtTrangThai.Text = row.Cells[2].Value?.ToString();

                // Cột 3: Sức chứa (Capacity)
                // Giả sử TextBox là txtSucChua
                txtSucChua.Text = row.Cells[3].Value?.ToString();
            }
        }
        private void ClearTableInputControls()
        {
            txtMaBan.Text = "";
            txtTenBan.Text = "";
            txtTrangThai.Text = "";
            txtSucChua.Text = "";
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DỮ LIỆU TỪ TEXTBOXES ⭐
            string maBan = txtMaBan.Text.Trim();
            string tenBan = txtTenBan.Text.Trim();
            string trangThai = txtTrangThai.Text.Trim();

            // Sức chứa (Capacity) cần chuyển đổi sang số nguyên (int)
            int sucChua;
            if (!int.TryParse(txtSucChua.Text.Trim(), out sucChua))
            {
                MessageBox.Show("Sức chứa phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra các trường bắt buộc (Mã bàn và Tên bàn)
            if (string.IsNullOrEmpty(maBan) || string.IsNullOrEmpty(tenBan))
            {
                MessageBox.Show("Mã bàn và Tên bàn không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. THỰC HIỆN INSERT SQL ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh INSERT INTO
                    // Lưu ý: Đảm bảo tên bảng và tên cột khớp với CSDL của bạn (Table, ID, Name, Status, Capacity)
                    string query = @"INSERT INTO [dbo].[Table] ( Name, Status, Capacity) 
                             VALUES (@Name, @Status, @Capacity)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Gán giá trị vào Parameters
                    cmd.Parameters.AddWithValue("@ID", maBan);
                    cmd.Parameters.AddWithValue("@Name", tenBan);
                    cmd.Parameters.AddWithValue("@Capacity", sucChua);

                    // Xử lý Trạng thái (có thể chấp nhận NULL hoặc mặc định)
                    if (string.IsNullOrEmpty(trangThai))
                        cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Status", trangThai);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    // ⭐ 3. THÔNG BÁO, REFRESH VÀ XÓA TEXTBOXES ⭐
                    if (result > 0)
                    {
                        MessageBox.Show($"Đã thêm Bàn '{tenBan}' (Mã: {maBan}) thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 1. Refresh DataGridView Bàn
                        LoadTableData();

                        // 2. Xóa nội dung trên các controls
                        ClearTableInputControls();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm bàn vào cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Xử lý lỗi trùng khóa chính (PRIMARY KEY violation)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show($"Lỗi: Mã bàn '{maBan}' đã tồn tại. Vui lòng chọn Mã bàn khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi SQL khi thêm bàn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DỮ LIỆU ĐẦU VÀO TỪ TEXTBOXES ⭐
            string maBan = txtMaBan.Text.Trim(); // Mã bàn (Khóa chính)
            string tenBan = txtTenBan.Text.Trim();
            string trangThai = txtTrangThai.Text.Trim();

            // Sức chứa (Capacity) cần chuyển đổi sang số nguyên (int)
            int sucChua;
            if (!int.TryParse(txtSucChua.Text.Trim(), out sucChua))
            {
                MessageBox.Show("Sức chứa phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra Mã bàn (ID) phải tồn tại
            if (string.IsNullOrEmpty(maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn cần cập nhật hoặc nhập Mã bàn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. HỎI XÁC NHẬN TỪ NGƯỜI DÙNG ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn cập nhật thông tin cho Bàn '{maBan}' không?",
                "Xác nhận Cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 3. THỰC HIỆN UPDATE SQL ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh UPDATE: Cập nhật tất cả các trường NGOẠI TRỪ ID
                    // Giả định tên bảng là [dbo].[Table]
                    string query = @"UPDATE [dbo].[Table] 
                             SET Name = @Name, 
                                 Status = @Status, 
                                 Capacity = @Capacity 
                             WHERE ID = @ID"; // Dùng ID làm điều kiện

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Gán giá trị vào Parameters
                    cmd.Parameters.AddWithValue("@Name", tenBan);
                    cmd.Parameters.AddWithValue("@Capacity", sucChua);

                    // Xử lý Trạng thái (có thể NULL)
                    if (string.IsNullOrEmpty(trangThai))
                        cmd.Parameters.AddWithValue("@Status", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Status", trangThai);

                    // Điều kiện WHERE (Khóa chính)
                    cmd.Parameters.AddWithValue("@ID", maBan);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();

                    // ⭐ 4. THÔNG BÁO VÀ REFRESH DỮ LIỆU ⭐
                    if (result > 0)
                    {
                        MessageBox.Show($"Cập nhật Bàn '{maBan}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 1. Refresh DataGridView Bàn
                        LoadTableData();

                        // 2. Xóa nội dung trên các controls (Tùy chọn)
                        ClearTableInputControls();
                    }
                    else
                    {
                        MessageBox.Show($"Không tìm thấy Bàn có Mã '{maBan}' để cập nhật hoặc không có thay đổi nào.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật bàn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DỮ LIỆU ĐẦU VÀO (ID) ⭐
            string maBan = txtMaBan.Text.Trim();

            if (string.IsNullOrEmpty(maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa hoặc nhập Mã bàn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. HỎI XÁC NHẬN (CẢNH BÁO VỀ DỮ LIỆU LIÊN QUAN) ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn XÓA Bàn có Mã '{maBan}' không?\n\nCảnh báo: Tất cả các Hóa Đơn liên quan đến bàn này cũng sẽ bị XÓA theo!",
                "Xác nhận Xóa Bàn và Dữ liệu liên quan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation // Dùng Exclamation để nhấn mạnh cảnh báo
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 3. THỰC HIỆN XÓA CASCADING BẰNG TRANSACTION ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            SqlTransaction transaction = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(); // Bắt đầu Transaction

                    // 1. Xóa các bản ghi liên quan (HoaDon)
                    string deleteInvoiceQuery = @"DELETE FROM Bills WHERE TableID = @TableID";
                    SqlCommand cmdInvoice = new SqlCommand(deleteInvoiceQuery, conn, transaction);
                    cmdInvoice.Parameters.AddWithValue("@TableID", maBan);
                    cmdInvoice.ExecuteNonQuery();

                    // 2. Xóa bản ghi gốc (Table)
                    string deleteTableQuery = @"DELETE FROM [dbo].[Table] WHERE ID = @TableID";
                    SqlCommand cmdTable = new SqlCommand(deleteTableQuery, conn, transaction);
                    cmdTable.Parameters.AddWithValue("@TableID", maBan);
                    int result = cmdTable.ExecuteNonQuery();

                    if (result > 0)
                    {
                        // ⭐ BƯỚC KHẮC PHỤC ⭐
                        transaction.Commit(); // Commit Transaction nếu thành công

                        MessageBox.Show($"Đã xóa Bàn có Mã '{maBan}' và các hóa đơn liên quan thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadTableData();
                        ClearTableInputControls();
                    }
                    else
                    {
                        // Nếu không tìm thấy bàn để xóa, chỉ Rollback và thông báo
                        transaction.Rollback();
                        MessageBox.Show($"Không tìm thấy Bàn có Mã '{maBan}' để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // ⭐ BƯỚC KHẮC PHỤC LỖI TRONG CATCH ⭐
                // Chỉ Rollback nếu transaction đã được khởi tạo và chưa bị hủy
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackEx)
                    {
                        // Xử lý lỗi Rollback (nếu có, thường hiếm)
                        MessageBox.Show($"Lỗi Rollback: {rollbackEx.Message}", "Lỗi Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                MessageBox.Show("Lỗi khi xóa bàn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // ⭐ 1. LẤY DỮ LIỆU ĐẦU VÀO (ID) ⭐
            string maBan = txtMaBan.Text.Trim();

            if (string.IsNullOrEmpty(maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa hoặc nhập Mã bàn.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ 2. HỎI XÁC NHẬN (CẢNH BÁO VỀ DỮ LIỆU LIÊN QUAN) ⭐
            DialogResult confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn XÓA Bàn có Mã '{maBan}' không?\n\nCảnh báo: Tất cả các Hóa Đơn liên quan đến bàn này cũng sẽ bị XÓA theo!",
                "Xác nhận Xóa Bàn và Dữ liệu liên quan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation // Dùng Exclamation để nhấn mạnh cảnh báo
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            // ⭐ 3. THỰC HIỆN XÓA CASCADING BẰNG TRANSACTION ⭐
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            SqlTransaction transaction = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    transaction = conn.BeginTransaction(); // Bắt đầu Transaction

                    // 1. Xóa các bản ghi liên quan (HoaDon)
                    string deleteInvoiceQuery = @"DELETE FROM Bills WHERE TableID = @TableID";
                    SqlCommand cmdInvoice = new SqlCommand(deleteInvoiceQuery, conn, transaction);
                    cmdInvoice.Parameters.AddWithValue("@TableID", maBan);
                    cmdInvoice.ExecuteNonQuery();

                    // 2. Xóa bản ghi gốc (Table)
                    string deleteTableQuery = @"DELETE FROM [dbo].[Table] WHERE ID = @TableID";
                    SqlCommand cmdTable = new SqlCommand(deleteTableQuery, conn, transaction);
                    cmdTable.Parameters.AddWithValue("@TableID", maBan);
                    int result = cmdTable.ExecuteNonQuery();

                    if (result > 0)
                    {
                        // ⭐ BƯỚC KHẮC PHỤC ⭐
                        transaction.Commit(); // Commit Transaction nếu thành công

                        MessageBox.Show($"Đã xóa Bàn có Mã '{maBan}' và các hóa đơn liên quan thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadTableData();
                        ClearTableInputControls();
                    }
                    else
                    {
                        // Nếu không tìm thấy bàn để xóa, chỉ Rollback và thông báo
                        transaction.Rollback();
                        MessageBox.Show($"Không tìm thấy Bàn có Mã '{maBan}' để xóa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                // ⭐ BƯỚC KHẮC PHỤC LỖI TRONG CATCH ⭐
                // Chỉ Rollback nếu transaction đã được khởi tạo và chưa bị hủy
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackEx)
                    {
                        // Xử lý lỗi Rollback (nếu có, thường hiếm)
                        MessageBox.Show($"Lỗi Rollback: {rollbackEx.Message}", "Lỗi Rollback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                MessageBox.Show("Lỗi khi xóa bàn: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BillsForm billsForm = new BillsForm();
            billsForm.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Diary diary = new Diary();
            diary.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AccountManagerForm accountForm = new AccountManagerForm(); // Tạo form mới
            accountForm.Show(); // Hiển thị form mới, KHÔNG ẩn form hiện tại
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnQuanLy_Click(object sender, EventArgs e)
        {
            btnTaiKhoan btnTaiKhoan = new btnTaiKhoan();
            btnTaiKhoan.ShowDialog();
        }
    }
}
