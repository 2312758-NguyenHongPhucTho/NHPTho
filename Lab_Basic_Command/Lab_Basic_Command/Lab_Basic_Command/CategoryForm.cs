using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lab_Basic_Command
{
    public partial class btnTaiKhoan : Form
    {
        public btnTaiKhoan()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // Tạo chuỗi kết nối tới cơ sở dữ liệu RestaurantManagement
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";

            // Tạo đối tượng kết nối
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            // Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            // Thiết lập lệnh truy vấn cho đối tượng Command
            string query = "SELECT ID, Name, Type FROM Category";
            sqlCommand.CommandText = query;

            // Mở kết nối tới cơ sở dữ liệu
            sqlConnection.Open();

            // Thực thi lệnh bằng phương thức ExecuteReader
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            // Gọi hàm hiển thị dữ liệu lên màn hình
            this.DisplayCategory(sqlDataReader);

            // Đóng kết nối
            sqlConnection.Close();
        }
        private void DisplayCategory(SqlDataReader reader)
        {
            // Xóa tất cả các dòng hiện tại
            lvCategory.Items.Clear();

            // Đọc một dòng dữ liệu
            while (reader.Read())
            {
                // Tạo một dòng mới trong ListView
                ListViewItem item = new ListViewItem(reader["ID"].ToString());

                // Thêm dòng mới vào ListView
                lvCategory.Items.Add(item);

                // Bổ sung các thông tin khác cho ListViewItem
                item.SubItems.Add(reader["Name"].ToString());
                item.SubItems.Add(reader["Type"].ToString());
            }
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text) || string.IsNullOrWhiteSpace(txtType.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Kiểm tra txtType có phải là số không
            if (!int.TryParse(txtType.Text, out int typeValue))
            {
                MessageBox.Show("Loại phải điền số");
                return;
            }

            // Chuỗi kết nối
            string connectionString = "server=DESKTOP-G37ME9E; database=RestaurantManagement; Integrated Security=true;";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // Câu lệnh SQL dùng tham số (tránh lỗi SQL Injection)
                string query = "INSERT INTO Category(Name, [Type]) VALUES (@Name, @Type)";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    // Truyền tham số an toàn
                    sqlCommand.Parameters.AddWithValue("@Name", txtCategoryName.Text);
                    sqlCommand.Parameters.AddWithValue("@Type", typeValue);

                    try
                    {
                        sqlConnection.Open();
                        int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

                        if (numOfRowsEffected == 1)
                        {
                            MessageBox.Show("Thêm nhóm món ăn thành công");

                            // Tải lại dữ liệu
                            btnLoad.PerformClick();

                            // Xóa các ô nhập
                            txtCategoryName.Text = "";
                            txtType.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
                    }
                }
            }
        }

        private void lvCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvCategory_Click(object sender, EventArgs e)
        {
            // Lấy dòng được chọn trong ListView
            ListViewItem item = lvCategory.SelectedItems[0];

            // Hiển thị dữ liệu lên Textbox
            txtCategoryID.Text = item.Text;
            txtCategoryName.Text = item.SubItems[1].Text;
            txtType.Text = item.SubItems[2].Text == "0" ? "Thức uống" : "Đồ ăn";

            // Hiển thị nút cập nhật và xóa
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng kết nối
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            // Sử dụng khối using để đảm bảo đối tượng kết nối và lệnh được giải phóng
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                // Tạo đối tượng thực thi lệnh
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                // 1. Dùng Tham số hóa (Parameterized Query) cho câu lệnh SQL
                // Tên cột [Type] được bao quanh bằng dấu ngoặc vuông [] vì 'Type' là từ khóa trong SQL
                sqlCommand.CommandText = "UPDATE Category SET Name = @Name, [Type] = @Type WHERE ID = @ID";

                // 2. Thêm các tham số
                // Tham số cho Name (nvarchar)
                sqlCommand.Parameters.AddWithValue("@Name", txtCategoryName.Text);

                // Tham số cho Type (int) - Đảm bảo giá trị là số nguyên
                int typeValue;
                if (int.TryParse(txtType.Text, out typeValue))
                {
                    sqlCommand.Parameters.AddWithValue("@Type", typeValue);
                }
                else
                {
                    MessageBox.Show("Giá trị Loại (Type) phải là một số nguyên.");
                    return; // Dừng thực thi nếu Type không hợp lệ
                }

                // Tham số cho ID (int) - Dùng cho mệnh đề WHERE
                int idValue;
                if (int.TryParse(txtCategoryID.Text, out idValue))
                {
                    sqlCommand.Parameters.AddWithValue("@ID", idValue);
                }
                else
                {
                    MessageBox.Show("ID nhóm món ăn không hợp lệ.");
                    return; // Dừng thực thi nếu ID không hợp lệ
                }

                // Mở kết nối tới cơ sở dữ liệu
                try
                {
                    sqlConnection.Open();

                    // Thực thi lệnh bằng phương thức ExecuteNonQuery
                    int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

                    // Xử lý kết quả
                    if (numOfRowsEffected == 1)
                    {
                        // Cập nhật lại dữ liệu trên ListView (Phần này giữ nguyên)
                        ListViewItem item = lvCategory.SelectedItems[0];
                        item.SubItems[1].Text = txtCategoryName.Text;
                        item.SubItems[2].Text = txtType.Text;

                        // Xóa các ô nhập
                        txtCategoryID.Text = "";
                        txtCategoryName.Text = "";
                        txtType.Text = "";

                        // Disable các nút xóa và cập nhật
                        btnUpdate.Enabled = false;
                        btnDelete.Enabled = false;

                        MessageBox.Show("Cập nhật nhóm món ăn thành công");
                    }
                    else
                    {
                        MessageBox.Show("Đã có lỗi xảy ra. Không có dòng nào được cập nhật. Vui lòng kiểm tra lại ID.");
                    }
                }
                catch (SqlException ex)
                {
                    // Bắt lỗi SQL cụ thể
                    MessageBox.Show("Lỗi SQL: " + ex.Message + "\nVui lòng kiểm tra cú pháp và dữ liệu nhập.");
                }
                catch (Exception ex)
                {
                    // Bắt các lỗi chung khác
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                }
                // Khối using tự động đóng kết nối, không cần sqlConnection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            {
                // Tạo đối tượng kết nối
                string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                // Tạo đối tượng thực thi lệnh
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                // Thiết lập lệnh truy vấn cho đối tượng Command
                sqlCommand.CommandText = "DELETE FROM Category " +
                                         "WHERE ID = " + txtCategoryID.Text;

                // Mở kết nối tới cơ sở dữ liệu
                sqlConnection.Open();

                // Thực thi lệnh bằng phương thức ExecuteNonQuery
                int numOfRowsEffected = sqlCommand.ExecuteNonQuery();

                // Đóng kết nối
                sqlConnection.Close();

                if (numOfRowsEffected == 1)
                {
                    // Cập nhật lại dữ liệu trên ListView
                    ListViewItem item = lvCategory.SelectedItems[0];
                    lvCategory.Items.Remove(item);

                    // Xóa các ô nhập
                    txtCategoryID.Text = "";
                    txtCategoryName.Text = "";
                    txtType.Text = "";

                    // Disable các nút xóa và cập nhật
                    btnUpdate.Enabled = false;
                    btnDelete.Enabled = false;

                    MessageBox.Show("Xóa nhóm món ăn thành công");
                }
                else
                {
                    MessageBox.Show("Đã có lỗi xảy ra. Vui lòng thử lại");
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            if (lvCategory.SelectedItems.Count > 0)
                btnDelete.PerformClick();
        }

        private void tsmViewFood_Click(object sender, EventArgs e)
        {
            if(txtCategoryID.Text!="")
            {
                FoodForm foodForm  = new FoodForm();
                foodForm.Show(this);
                foodForm.LoadFood(Convert.ToInt32(txtCategoryID.Text));
            }
        }

        private void btnXemBill_Click(object sender, EventArgs e)
        {
            BillsForm billForm = new BillsForm(); // Tạo form mới
            billForm.Show(); // Hiển thị form mới, KHÔNG ẩn form hiện tại
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccountManagerForm accountForm = new AccountManagerForm(); // Tạo form mới
            accountForm.Show(); // Hiển thị form mới, KHÔNG ẩn form hiện tại
        }

        private void btnTaiKhoan_Load(object sender, EventArgs e)
        {

        }
    }
}
