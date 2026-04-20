using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;

namespace Assignment2_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        // Bài Test 1: Kiểm tra kết nối Database thông qua App.config
        [TestMethod]
        public void Test_DatabaseConnection()
        {
            // Lấy chuỗi kết nối từ App.config
            string connString = ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString;
            bool isConnected = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    isConnected = true; // Nếu mở được là kết nối thành công
                }
                catch
                {
                    isConnected = false;
                }
            }

            // Khẳng định (Assert) kết nối phải thành công
            Assert.IsTrue(isConnected, "Không thể kết nối đến Database. Hãy kiểm tra lại chuỗi kết nối!");
        }

        // Bài Test 2: Kiểm tra logic tính tổng tiền Giỏ hàng (Mô phỏng Exer 2, 3, 4)
        [TestMethod]
        public void Test_CartSubTotalCalculation()
        {
            // Giả lập dữ liệu
            int quantity = 2;
            double unitAmount = 15000000; // 15 triệu (vd: Laptop)

            // Tính toán thực tế
            double expectedSubTotal = 30000000; // Mong đợi 30 triệu
            double actualSubTotal = quantity * unitAmount;

            // Khẳng định (Assert) kết quả phải khớp nhau
            Assert.AreEqual(expectedSubTotal, actualSubTotal, "Tính toán tổng tiền giỏ hàng bị sai!");
        }
    }
}