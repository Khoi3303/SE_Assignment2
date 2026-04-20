using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OrderDAL : DBConnection
    {
        // Hàm lưu Order và trả về OrderID vừa được tạo
        public int InsertOrder(OrderDTO order)
        {
            // SCOPE_IDENTITY() trả về ID tự tăng vừa được insert
            string query = "INSERT INTO [Order] (OrderDate, AgentID) VALUES (@date, @agentId); SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@date", order.OrderDate);
            cmd.Parameters.AddWithValue("@agentId", order.AgentID);

            conn.Open();
            // Lấy ra ID vừa tạo
            int insertedID = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            return insertedID;
        }

        // Hàm lưu từng dòng chi tiết đơn hàng
        public bool InsertOrderDetail(OrderDetailDTO detail)
        {
            string query = "INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES (@orderId, @itemId, @qty, @price)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@orderId", detail.OrderID);
            cmd.Parameters.AddWithValue("@itemId", detail.ItemID);
            cmd.Parameters.AddWithValue("@qty", detail.Quantity);
            cmd.Parameters.AddWithValue("@price", detail.UnitAmount);

            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return result > 0;
        }
        // Lấy chi tiết của đơn hàng mới nhất vừa được lưu
        public DataTable GetLastOrderDetails()
        {
            string query = @"SELECT i.ItemName, od.Quantity, od.UnitAmount AS Price, (od.Quantity * od.UnitAmount) AS SubTotal 
                     FROM OrderDetail od 
                     JOIN Item i ON od.ItemID = i.ItemID 
                     WHERE od.OrderID = (SELECT MAX(OrderID) FROM [Order])";
            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}