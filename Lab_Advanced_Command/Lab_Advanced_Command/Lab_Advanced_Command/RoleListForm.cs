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
    public partial class RoleListForm : Form
    {
        string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

        public RoleListForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadRoleData();
        }
        private void LoadRoleData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Lấy các cột đúng như trong bảng bạn gửi ảnh
                    string query = "SELECT ID, RoleName, Path, Notes FROM Role";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvRole.DataSource = dt; // Gán dữ liệu cho DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Role: " + ex.Message);
            }
        }

        // (Tùy chọn) Tự động load khi mở form
        private void frmRole_Load(object sender, EventArgs e)
        {
            LoadRoleData();
        }

        private void dgvRole_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo người dùng click vào một hàng hợp lệ (không phải header)
            if (e.RowIndex >= 0)
            {
                // Lấy hàng hiện tại được click
                DataGridViewRow row = dgvRole.Rows[e.RowIndex];

                // Dựa trên thứ tự hiển thị trong bảng (từ trái sang phải)

                // 1. Hiển thị ID (Cột 1 -> Index 0)
                // Giả sử: TextBox ID là txtID
                txtID.Text = row.Cells[0].Value.ToString();

                // 2. Hiển thị RoleName (Cột 2 -> Index 1)
                // Giả sử: TextBox RoleName là txtRoleName
                txtRoleName.Text = row.Cells[1].Value.ToString();

                // 3. Hiển thị Path (Cột 3 -> Index 2)
                // Giả sử: TextBox Path là txtPath
                txtPath.Text = row.Cells[2].Value.ToString();

                // 4. Hiển thị Notes (Cột 4 -> Index 3)
                // Giả sử: TextBox Notes là txtNotes
                txtNotes.Text = row.Cells[3].Value.ToString();

                // Lưu ý: Nếu cột nào có giá trị NULL trong database, bạn có thể cần kiểm tra
                // và thay thế bằng chuỗi rỗng để tránh lỗi:
                /*
                txtNotes.Text = row.Cells[3].Value != DBNull.Value ? 
                                row.Cells[3].Value.ToString() : 
                                string.Empty;
                */
            }
        }

        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(txtRoleName.Text)) // Giả sử TextBox RoleName là txtRoleName
            {
                MessageBox.Show("Vui lòng nhập Tên vai trò (RoleName).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Câu lệnh SQL INSERT
                    string query = "INSERT INTO Role (RoleName, Path, Notes) VALUES (@roleName, @path, @notes)";

                    // Giả định: Bảng lưu trữ vai trò là 'Role'
                    // Tên các cột trong bảng là: RoleName, Path, Notes

                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Khai báo và gán giá trị cho tham số từ các TextBox
                    cmd.Parameters.Add("@roleName", SqlDbType.NVarChar, 200).Value = txtRoleName.Text; // Giả sử txtRoleName
                    cmd.Parameters.Add("@path", SqlDbType.NVarChar, 500).Value = txtPath.Text; // Giả sử txtPath
                    cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 1000).Value = txtNotes.Text; // Giả sử txtNotes

                    conn.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm vai trò mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 2. Tải lại dữ liệu lên DataGridView để hiển thị vai trò vừa thêm
                        LoadRoleData();

                        // 3. Xóa nội dung TextBox sau khi thêm thành công
                        txtRoleName.Clear();
                        txtPath.Clear();
                        txtNotes.Clear();
                        txtID.Clear(); // Giả sử ID là tự động tăng và chỉ hiển thị
                    }
                    else
                    {
                        MessageBox.Show("Thêm vai trò thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi thêm dữ liệu:\n{ex.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
