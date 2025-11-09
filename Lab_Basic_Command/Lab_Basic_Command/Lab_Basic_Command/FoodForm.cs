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
    public partial class FoodForm : Form
    {
        string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

        private SqlConnection sqlConnection;
        SqlConnection connection;
        private SqlDataAdapter da;
        private DataTable dt;

        public FoodForm()
        {
            InitializeComponent();
        }
        public void LoadFood(int categoryID)
        {
            
            // Tạo đối tượng kết nối
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            // Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            // Thiết lập lệnh truy vấn cho đối tượng Command
            sqlCommand.CommandText = "SELECT Name FROM Category WHERE ID = " + categoryID;

            // Mở kết nối tới cơ sở dữ liệu
            sqlConnection.Open();

            // Gán tên nhóm sản phẩm cho tiêu đề
            string catName = sqlCommand.ExecuteScalar().ToString();
            this.Text = "Danh sách các món ăn thuộc nhóm: " + catName;

            sqlCommand.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = " + categoryID;

            // Tạo đối tượng DataAdapter
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);

            // Tạo DataTable để chứa dữ liệu
            DataTable dt = new DataTable("Food");
            da.Fill(dt);

            // Hiển thị danh sách món ăn lên Form
            dgvFood.DataSource = dt;

            // Đóng kết nối và giải phóng bộ nhớ
            sqlConnection.Close();
            sqlConnection.Dispose();
            da.Dispose();
          
           
         
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Food", connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);

                DataTable dt = (DataTable)dgvFood.DataSource;

                dgvFood.EndEdit();
                int rowsAffected = da.Update(dt);

                MessageBox.Show($"Đã lưu thành công {rowsAffected} dòng dữ liệu.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.AcceptChanges();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvFood.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvFood.CurrentRow.Cells[0].Value); // Lấy giá trị ở cột đầu tiên (Mã món)


            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa món này (và các dữ liệu liên quan)?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            try
            {
                string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Xóa dữ liệu liên quan trong BillDetails
                    string deleteBillDetailsQuery = "DELETE FROM BillDetails WHERE FoodID = @FoodID";
                    using (SqlCommand cmd1 = new SqlCommand(deleteBillDetailsQuery, conn))
                    {
                        cmd1.Parameters.AddWithValue("@FoodID", id);
                        cmd1.ExecuteNonQuery();
                    }

                    // Xóa món ăn trong bảng Food
                    string deleteFoodQuery = "DELETE FROM Food WHERE ID = @FoodID"; // Đổi "ID" theo đúng tên cột trong SQL
                    using (SqlCommand cmd2 = new SqlCommand(deleteFoodQuery, conn))
                    {
                        cmd2.Parameters.AddWithValue("@FoodID", id);
                        cmd2.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadData()
        {
            connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM Food";

            da = new SqlDataAdapter(query, connection);
            dt = new DataTable();
            da.Fill(dt);

            dgvFood.AutoGenerateColumns = true;
            dgvFood.DataSource = dt;
        }

        private void FoodForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Food", connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(da);

                DataTable dt = (DataTable)dgvFood.DataSource;

                dgvFood.EndEdit();
                int rowsAffected = da.Update(dt);

                MessageBox.Show($"Đã lưu thành công {rowsAffected} dòng dữ liệu.",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.AcceptChanges();
            }

        }
    }
}
