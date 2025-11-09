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
using Microsoft.VisualBasic;

namespace Lab_Advanced_Command
{
    public partial class FoodInfoForm : Form
    {
        public FoodInfoForm()
        {
            InitializeComponent();
            nudPrice.Maximum = decimal.MaxValue;

        }

        private void FoodInfoForm_Load(object sender, EventArgs e)
        {
            this.InitValues();
        }
        private void InitValues()
        {
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name FROM Category";
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            adapter.Fill(ds, "Category");
            cbbCatName.DataSource = ds.Tables["Category"];
            cbbCatName.DisplayMember = "Name";
            cbbCatName.ValueMember = "ID";
            conn.Close();
            conn.Dispose();
        }
        private void ResetText()
        {
            txtID.ResetText();
            txtName.ResetText();
            txtNotes.ResetText();
            txtUnit.ResetText();
            nudPrice.ResetText();
            cbbCatName.ResetText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Tạo đối tượng thực thi lệnh
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "EXECUTE InsertFood @id OUTPUT, @name, @unit, @foodCategoryID, @price, @notes";

                    // Khai báo các tham số
                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 1000);
                    cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 100);
                    cmd.Parameters.Add("@foodCategoryID", SqlDbType.Int);
                    cmd.Parameters.Add("@price", SqlDbType.Int);
                    cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 3000);

                    // Gán giá trị cho tham số
                    cmd.Parameters["@name"].Value = txtName.Text;
                    cmd.Parameters["@unit"].Value = txtUnit.Text;
                    cmd.Parameters["@foodCategoryID"].Value = cbbCatName.SelectedValue;
                    cmd.Parameters["@price"].Value = nudPrice.Value;
                    cmd.Parameters["@notes"].Value = txtNotes.Text;

                    // Mở kết nối
                    conn.Open();

                    // Thực thi truy vấn
                    int numRowsAffected = cmd.ExecuteNonQuery();

                    // Kiểm tra kết quả
                    if (numRowsAffected > 0)
                    {
                        string foodID = cmd.Parameters["@id"].Value.ToString();
                        MessageBox.Show($"Successfully added new food. Food ID = {foodID}", "Message");
                        this.ResetText();
                    }
                    else
                    {
                        MessageBox.Show("Adding food failed", "Message");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "SQL Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        public void DisplayFoodInfo(DataRowView rowView)
        {
            try
            {
                // Gán giá trị từ dòng dữ liệu vào các ô nhập liệu
                txtID.Text = rowView["ID"].ToString();
                txtName.Text = rowView["Name"].ToString();
                txtUnit.Text = rowView["Unit"].ToString();
                txtNotes.Text = rowView["Notes"].ToString();
                nudPrice.Value = Convert.ToDecimal(rowView["Price"]);

                // Thiết lập lại combobox nhóm món ăn
                cbbCatName.SelectedIndex = -1;

                // Chọn nhóm món ăn tương ứng với FoodCategoryID
                for (int index = 0; index < cbbCatName.Items.Count; index++)
                {
                    DataRowView cat = cbbCatName.Items[index] as DataRowView;
                    if (cat != null && cat["ID"].ToString() == rowView["FoodCategoryID"].ToString())
                    {
                        cbbCatName.SelectedIndex = index;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                this.Close();
            }
        }

        private void btnUppdateFood_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Tạo đối tượng thực thi lệnh kiểu Stored Procedure
                    SqlCommand cmd = new SqlCommand("UpdateFood", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                   

                        // Thêm tham số vào lệnh (ID, Name, Unit, FoodCategoryID, Price, Notes)
                        

                        // Thêm tham số vào lệnh
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = int.Parse(txtID.Text);
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 1000).Value = txtName.Text;
                    cmd.Parameters.Add("@unit", SqlDbType.NVarChar, 100).Value = txtUnit.Text;
                    cmd.Parameters.Add("@foodCategoryID", SqlDbType.Int).Value = cbbCatName.SelectedValue;
                    cmd.Parameters.Add("@price", SqlDbType.Int).Value = Convert.ToInt32(nudPrice.Value);
                    cmd.Parameters.Add("@notes", SqlDbType.NVarChar, 3000).Value = txtNotes.Text;

                    // Mở kết nối
                    conn.Open();

                    // Thực thi stored procedure
                    int numRowAffected = cmd.ExecuteNonQuery();

                    // Kiểm tra kết quả
                    if (numRowAffected > 0)
                        MessageBox.Show("✅ Cập nhật món ăn thành công!", "Thông báo");
                    else
                        MessageBox.Show("⚠️ Không có món ăn nào được cập nhật.", "Thông báo");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL: " + ex.Message, "Lỗi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Nhập tên nhóm món ăn mới
            string newCatName = Interaction.InputBox(
                "Nhập tên nhóm món ăn mới:", "Thêm nhóm món ăn");

            // Nếu người dùng không nhập thì thoát
            if (string.IsNullOrWhiteSpace(newCatName)) return;

            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Category(Name, Type) VALUES(@name, 1)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", newCatName);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Gọi lại InitValues() để nạp lại danh sách nhóm món ăn
            InitValues();

            // Tự động chọn nhóm vừa thêm
            cbbCatName.SelectedIndex = cbbCatName.FindStringExact(newCatName);
        }
    }

}