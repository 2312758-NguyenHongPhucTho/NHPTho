using Lab_Basic_Command;
using Microsoft.VisualBasic;

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

namespace Lab_Advanced_Command
{
    public partial class FoodForm : Form
    {
        private DataTable foodTable;
        public FoodForm()
        {
            InitializeComponent();
        }

        private void LoadCategory()
        {
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT ID, Name FROM Category";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            // Mở kết nối
            conn.Open();

            // Lấy dữ liệu từ csdl đưa vào DataTable
            adapter.Fill(dt);

            // Đóng kết nối và giải phóng bộ nhớ
            conn.Close();
            conn.Dispose();

            // Đưa dữ liệu vào Combo Box
            cbbCategory.DataSource = dt;

            // Hiển thị tên nhóm sản phẩm
            cbbCategory.DisplayMember = "Name";

            // Nhưng khi lấy giá trị thì lấy ID của nhóm
            cbbCategory.ValueMember = "ID";
        }
        private void FoodForm_Load(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbCategory.SelectedIndex == -1) return;

            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Food WHERE FoodCategoryID = @categoryId";
                cmd.Parameters.Add("@categoryId", SqlDbType.Int);

                if (cbbCategory.SelectedValue is DataRowView)
                {
                    DataRowView rowView = cbbCategory.SelectedValue as DataRowView;
                    cmd.Parameters["@categoryId"].Value = rowView["ID"];
                }
                else
                {
                    cmd.Parameters["@categoryId"].Value = cbbCategory.SelectedValue ?? DBNull.Value;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                foodTable = new DataTable();

                conn.Open();
                adapter.Fill(foodTable);

                dgvFoodList.DataSource = foodTable;
                lblQuantity.Text = foodTable.Rows.Count.ToString();
                lblCatName.Text = cbbCategory.Text;
            }   
        }

        private void tsmCalculateQuantity_Click(object sender, EventArgs e)
        {
            string connectionString = "server= DESKTOP-G37ME9E; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT @numSaleFood = SUM(Quantity) FROM BillDetails WHERE FoodID = @foodId";

            // Lấy thông tin sản phẩm được chọn
            if (dgvFoodList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFoodList.SelectedRows[0];
                DataRowView rowView = selectedRow.DataBoundItem as DataRowView;

                // Truyền tham số
                cmd.Parameters.Add("@foodId", SqlDbType.Int);
                cmd.Parameters["@foodId"].Value = rowView["ID"];

                cmd.Parameters.Add("@numSaleFood", SqlDbType.Int);
                cmd.Parameters["@numSaleFood"].Direction = ParameterDirection.Output;

                // Mở kết nối CSDL
                conn.Open();

                // Thực thi truy vấn và lấy dữ liệu từ tham số
                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["@numSaleFood"].Value.ToString();

                MessageBox.Show("Tổng số lượng món " + rowView["Name"] +
                                " đã bán là: " + result + " " + rowView["Unit"]);

                // Đóng kết nối CSDL
                conn.Close();
            }

            cmd.Dispose();
            conn.Dispose();
        }

        private void tsmAddFood_Click(object sender, EventArgs e)
        {
            // Mở form thêm món ăn mới
            FoodInfoForm foodForm = new FoodInfoForm();
            foodForm.FormClosed += new FormClosedEventHandler(foodForm_FormClosed);
            foodForm.Show(this);
        }
        private void foodForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Cập nhật lại danh mục món ăn sau khi đóng form
            int index = cbbCategory.SelectedIndex;
            cbbCategory.SelectedIndex = -1;
            cbbCategory.SelectedIndex = index;
        }

        private void tsmUpdateFood_Click(object sender, EventArgs e)
        {
            // Lấy thông tin sản phẩm được chọn
            if (dgvFoodList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvFoodList.SelectedRows[0];
                DataRowView rowView = selectedRow.DataBoundItem as DataRowView;

                if (rowView != null)
                {
                    // Mở form chỉnh sửa món ăn
                    FoodInfoForm foodForm = new FoodInfoForm();
                    foodForm.FormClosed += new FormClosedEventHandler(foodForm_FormClosed);
                    foodForm.Show(this);

                    // Hiển thị thông tin món ăn trong form
                    foodForm.DisplayFoodInfo(rowView);
                }
            }
            else
            {
                MessageBox.Show("Please select a food item to update!", "Notice");
            }
        }

        private void txtTimName_TextChanged(object sender, EventArgs e)
        {
            if (foodTable == null) return;
            string fillterExpression = "Name like '%" + txtTimName.Text + "%'";
            string sortExpresstion = "Price DESC";
            DataViewRowState rowStateFillter = DataViewRowState.OriginalRows;
            DataView foodView = new DataView(foodTable,fillterExpression,sortExpresstion,rowStateFillter);
            dgvFoodList.DataSource = foodView;
        }

        private void tsmSeperator_Click(object sender, EventArgs e)
        {
            OrdersForm ordersForm = new OrdersForm();

            // Mở form OrdersForm dạng hộp thoại (có thể đóng quay lại)
            ordersForm.ShowDialog(this);
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountManagerForm frm = new AccountManagerForm();
            frm.ShowDialog();
        }

        private void dgvFoodList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
