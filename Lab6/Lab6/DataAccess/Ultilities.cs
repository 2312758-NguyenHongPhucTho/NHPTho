using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DataAccess
{
    public class Ultilities
    {
        // Lấy chuỗi kết nối từ tập tin App.Config
        private static string StrName = "ConnectionStringName";
        public static string ConnectionString = ConfigurationManager.ConnectionStrings[StrName].ConnectionString;

        // Biến lưu tên thủ tục và bảng trong cơ sở dữ liệu
        // Biến bảng Food
        public static string Food_GetAll = "Food_GetAll";
        public static string Food_InsertUpdateDelete = "Food_InsertUpdateDelete";

        // Biến bảng Category
        public static string Category_GetAll = "Category_GetAll";
        public static string Category_InsertUpdateDelete = "Category_InsertUpdateDelete";

        // Biến bảng Table
        public static string Table_GetAll = "Table_GetAll";
        public static string Table_InsertUpdateDelete = "Table_InsertUpdateDelete";

        // Biến bảng Bills
        public static string Bills_GetAll = "Bills_GetAll";
        public static string Bills_InsertUpdateDelete = "Bills_InsertUpdateDelete";

        // Biến bảng BillDetails
        public static string BillDetails_GetAll = "BillDetails_GetAll";
        public static string BillDetails_InsertUpdateDelete = "BillDetails_InsertUpdateDelete";

        // Biến bảng Account
        public static string Account_GetAll = "Account_GetAll";
        public static string Account_InsertUpdateDelete = "Account_InsertUpdateDelete";

        // Biến bảng Role
        public static string Role_GetAll = "Role_GetAll";
        public static string Role_InsertUpdateDelete = "Role_InsertUpdateDelete";

        // Biến bảng RoleAccount
        public static string RoleAccount_GetAll = "RoleAccount_GetAll";
        public static string RoleAccount_InsertUpdateDelete = "RoleAccount_InsertUpdateDelete";

        // Biến các hàm và báo cáo
        public static string GetTotalSalesByDate = "GetTotalSalesByDate";
        public static string GetFoodQuantityByDate = "GetFoodQuantityByDate";
        public static string SalesReport_ByCategory = "SalesReport_ByCategory";

        // Biến cho các stored procedures dùng chung
        public static string GetByID = "_GetByID";
        public static string GetAll = "_GetAll";
        public static string Delete = "_Delete";

        // Các hàm tiện ích hỗ trợ CRUD cơ bản

        /// <summary>
        /// Lấy tất cả dữ liệu từ một bảng
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <returns>Chuỗi thủ tục</returns>
        public static string GetAllFromTable(string tableName)
        {
            return $"{GetAll} {tableName}";
        }

        /// <summary>
        /// Lấy dữ liệu theo ID
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="id">ID cần lấy</param>
        /// <returns>Chuỗi thủ tục</returns>
        public static string GetById(string tableName, int id)
        {
            return $"{GetByID} {tableName}, {id}";
        }

        /// <summary>
        /// Xóa dữ liệu theo ID
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="id">ID cần xóa</param>
        /// <returns>Chuỗi thủ tục</returns>
        public static string DeleteById(string tableName, int id)
        {
            return $"{Delete} {tableName}, {id}";
        }
    }
}
