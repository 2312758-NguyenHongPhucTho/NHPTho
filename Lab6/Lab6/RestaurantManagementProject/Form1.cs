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
    public partial class Form1 : Form
    {
        // Danh sách toàn cục bảng Category
        List<Category> listCategory = new List<Category>();
        // Danh sách toàn cục bảng Food
        List<Food> listFood = new List<Food>();
        // Đối tượng Food đang chọn hiện hành
        Food foodCurrent = new Food();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategory();
            // Đổ dữ liệu vào ListView
            LoadFoodDataToListView();
        }
        private void LoadCategory()
        {
            // Khởi tạo đối tượng CategoryBL từ tầng BusinessLogic
            CategoryBL categoryBL = new CategoryBL();

            // Lấy dữ liệu danh mục từ tầng BusinessLogic
            listCategory = categoryBL.GetAll();

            // Gán dữ liệu vào ComboBox với ValueMember là ID và DisplayMember là Name
            cbbCategory.DataSource = listCategory;
            cbbCategory.ValueMember = "ID";
            cbbCategory.DisplayMember = "Name";
        }
        public int InsertFood()
        {
            // Khai báo đối tượng Food từ tầng DataAccess
            Food food = new Food();
            food.ID = 0;

            // Kiểm tra nếu các ô nhập khác rỗng
            if (string.IsNullOrEmpty(txtName?.Text) || string.IsNullOrEmpty(txtUnit?.Text) || string.IsNullOrEmpty(txtPrice?.Text))
            {
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
            }
            else
            {
                // Nhận giá trị Name, Unit, và Notes từ người dùng nhập vào
                food.Name = txtName?.Text ?? string.Empty;
                food.Unit = txtUnit?.Text ?? string.Empty;
                food.Notes = txtNotes?.Text ?? string.Empty;

                // Giá trị price là giá trị số nên cần bắt lỗi khi người dùng nhập sai
                int price = 0;
                if (!int.TryParse(txtPrice?.Text, out price))
                {
                    price = 0; // Nếu không thể chuyển đổi, gán giá trị mặc định
                }
                food.Price = price;

                // Giá trị FoodCategoryID được lấy từ ComboBox
                int foodCategoryId = 0;
                if (!int.TryParse(cbbCategory?.SelectedValue?.ToString(), out foodCategoryId))
                {
                    MessageBox.Show("Giá trị danh mục không hợp lệ.");
                    return -1;
                }
                food.FoodCategoryID = foodCategoryId;

                // Khai báo đối tượng FoodBL từ tầng Business
                FoodBL foodBL = new FoodBL();
                // Chèn dữ liệu vào bảng
                return foodBL.Insert(food);
            }
            return -1;
        }
        private void LoadFoodDataToListView()
        {
            // Khởi tạo đối tượng FoodBL từ tầng BusinessLogic
            FoodBL foodBL = new FoodBL();

            // Lấy danh sách món ăn từ tầng BusinessLogic
            listFood = foodBL.GetAll();

            // Biến số thứ tự cho từng dòng
            int count = 1;

            // Xoá dữ liệu cũ trong ListView
            lsvFood.Items.Clear();

            // Duyệt danh sách dữ liệu để hiển thị trong ListView
            foreach (var food in listFood)
            {
                // Kiểm tra nếu dữ liệu món ăn là null
                if (food == null)
                {
                    continue;
                }

                // Tạo một dòng trong ListView, bắt đầu bằng số thứ tự
                ListViewItem item = new ListViewItem(count.ToString());

                // Thêm các cột dữ liệu
                item.SubItems.Add(food.Name ?? "N/A"); // Tên món ăn (hoặc "N/A" nếu null)
                item.SubItems.Add(food.Unit ?? "N/A"); // Đơn vị (hoặc "N/A" nếu null)
                item.SubItems.Add(food.Price.ToString("N0")); // Giá món ăn (định dạng số)

                // Hiển thị tên danh mục thay vì ID
                var category = listCategory.Find(x => x.ID == food.FoodCategoryID);
                item.SubItems.Add(category?.Name ?? "Unknown Category");

                // Ghi chú món ăn (hoặc "No notes" nếu null)
                item.SubItems.Add(food.Notes ?? "No notes");

                // Thêm dòng vào ListView
                lsvFood.Items.Add(item);

                // Tăng số thứ tự
                count++;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra món ăn hiện tại
                if (foodCurrent == null || foodCurrent.ID <= 0)
                {
                    MessageBox.Show("Vui lòng chọn món ăn để xoá.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận từ người dùng
                if (MessageBox.Show("Bạn có chắc chắn muốn xoá món ăn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    int result = new FoodBL().Delete(foodCurrent.ID);

                    // Kiểm tra kết quả
                    if (result > 0)
                    {
                        MessageBox.Show("Xoá món ăn thành công!");
                        LoadFoodDataToListView();
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

        private void btnClear_Click(object sender, EventArgs e)
        {

            txtName.Text = "";
            txtPrice.Text = "0";
            txtUnit.Text = "";
            txtNotes.Text = "";
            // Thiết lập index = 0 cho ComboBox
            if (cbbCategory.Items.Count > 0)
                cbbCategory.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra giá trị của txtName
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thực phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra giá trị SelectedValue của ComboBox
                if (cbbCategory.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn danh mục hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Food từ dữ liệu nhập
                Food food = new Food
                {
                    Name = txtName.Text.Trim(),
                    Unit = txtUnit.Text.Trim(),
                    FoodCategoryID = Convert.ToInt32(cbbCategory.SelectedValue),
                    Price = int.TryParse(txtPrice.Text.Trim(), out int price) ? price : 0,
                    Notes = txtNotes.Text.Trim()
                };

                // Gọi phương thức thêm dữ liệu
                int result = new FoodBL().Insert(food);

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                    LoadFoodDataToListView();
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
                // Kiểm tra món ăn hiện tại
                if (foodCurrent == null || foodCurrent.ID <= 0)
                {
                    MessageBox.Show("Vui lòng chọn món ăn để cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra giá trị của txtName
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thực phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng Food từ dữ liệu nhập
                Food food = new Food
                {
                    ID = foodCurrent.ID,
                    Name = txtName.Text.Trim(),
                    Unit = txtUnit.Text.Trim(),
                    FoodCategoryID = Convert.ToInt32(cbbCategory.SelectedValue),
                    Price = int.TryParse(txtPrice.Text.Trim(), out int price) ? price : 0,
                    Notes = txtNotes.Text.Trim()
                };

                // Gọi phương thức cập nhật dữ liệu
                int result = new FoodBL().Update(food);

                // Kiểm tra kết quả
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                    LoadFoodDataToListView();
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
        public int UpdateFood()
        {
            // Kiểm tra nếu đối tượng Food hiện hành có giá trị
            if (foodCurrent == null)
            {
                MessageBox.Show("Không có dữ liệu Food hiện hành để cập nhật.");
                return -1;
            }

            // Khai báo đối tượng Food và lấy đối tượng hiện hành
            Food food = foodCurrent;

            // Kiểm tra nếu các ô nhập khác rỗng
            if (string.IsNullOrEmpty(txtName?.Text) || string.IsNullOrEmpty(txtUnit?.Text) || string.IsNullOrEmpty(txtPrice?.Text))
            {
                MessageBox.Show("Chưa nhập dữ liệu cho các ô, vui lòng nhập lại");
            }
            else
            {
                // Nhận giá trị Name, Unit, và Notes khi người dùng sửa
                food.Name = txtName?.Text ?? string.Empty;
                food.Unit = txtUnit?.Text ?? string.Empty;
                food.Notes = txtNotes?.Text ?? string.Empty;

                // Giá trị price là giá trị số nên cần bắt lỗi khi người dùng nhập sai
                int price = 0;
                if (!int.TryParse(txtPrice?.Text, out price))
                {
                    price = 0; // Nếu không thể chuyển đổi, gán giá trị mặc định
                }
                food.Price = price;

                // Giá trị FoodCategoryID được lấy từ ComboBox
                int foodCategoryId = 0;
                if (!int.TryParse(cbbCategory?.SelectedValue?.ToString(), out foodCategoryId))
                {
                    MessageBox.Show("Giá trị danh mục không hợp lệ.");
                    return -1;
                }
                food.FoodCategoryID = foodCategoryId;

                // Khai báo đối tượng FoodBL từ tầng Business
                FoodBL foodBL = new FoodBL();

                // Cập nhật dữ liệu trong bảng
                return foodBL.Update(food);
            }

            return -1;
        }
        private void lsvFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < lsvFood.Items.Count; i++)
            {
                // Nếu có dòng được chọn thì lấy dòng đó
                if (lsvFood.Items[i].Selected)
                {
                    // Lấy các tham số và gán dữ liệu vào các ô
                    foodCurrent = listFood[i];
                    txtName.Text = foodCurrent.Name;
                    txtUnit.Text = foodCurrent.Unit;
                    txtPrice.Text = foodCurrent.Price.ToString();
                    txtNotes.Text = foodCurrent.Notes;
                    // Lấy index của Combobox theo FoodCategoryID
                    cbbCategory.SelectedIndex = listCategory.FindIndex(x => x.ID == foodCurrent.FoodCategoryID);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            frmAccount fr = new frmAccount();
            fr.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmBills fr = new frmBills();
            fr.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           

        }
    }
}
