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
        // Sử dụng TrustServerCertificate=True cho môi trường Local, hoặc có thể dùng Ultilities.ConnectionString đã tạo trước đó.
        // Tạm thời giữ nguyên chuỗi kết nối cục bộ của bạn.
        string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true; TrustServerCertificate=True;";

        public RoleListForm()
        {
            InitializeComponent();

            // Bước 1: Gọi hàm LoadRoleData() ngay trong hàm khởi tạo (Constructor) 
            // hoặc trong sự kiện Load của Form. Gọi trong Constructor sẽ đảm bảo dữ liệu 
            // được load sớm nhất có thể.
            LoadRoleData();
        }

        // Sự kiện Load của Form (Được gọi sau Constructor và trước khi Form hiển thị)
        private void frmRole_Load(object sender, EventArgs e)
        {
            // Nếu đã gọi trong Constructor (như ở trên), bạn có thể bỏ trống hoặc gọi lại ở đây.
            // Nếu bạn thích cách truyền thống, hãy gọi LoadRoleData() ở đây.
            // LoadRoleData(); 
        }

        private void LoadRoleData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Sử dụng Stored Procedure đã tạo trước đó là tốt nhất:
                    // string query = "EXEC Role_GetAll"; 
                    // Nhưng ở đây ta dùng query thường để đơn giản hóa.
                    string query = "SELECT ID, RoleName, Path, Notes FROM Role";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Gán dữ liệu cho DataGridView
                    dgvRole.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Role: " + ex.Message, "Lỗi kết nối CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút tải lại dữ liệu (Refresh Button) vẫn giữ nguyên
        private void button1_Click(object sender, EventArgs e)
        {
            LoadRoleData();
        }

        private void dgvRole_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Hàm này có thể để trống hoặc dùng cho các logic tương tác click chuột vào ô
        }
    }
}