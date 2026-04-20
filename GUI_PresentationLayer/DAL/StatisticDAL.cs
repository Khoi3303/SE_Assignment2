using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class StatisticDAL : DBConnection
    {
        // 1. Lọc top 5 sản phẩm bán chạy nhất
        public DataTable GetBestItems()
        {
            string query = @"SELECT TOP 5 i.ItemName, SUM(od.Quantity) AS TotalSold 
                             FROM Item i 
                             JOIN OrderDetail od ON i.ItemID = od.ItemID 
                             GROUP BY i.ItemName 
                             ORDER BY TotalSold DESC";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // 2. Lọc các sản phẩm mà một Đại lý (Agent) đã mua
        public DataTable GetItemsByAgent(int agentId)
        {
            string query = @"SELECT i.ItemName, SUM(od.Quantity) AS TotalBought 
                             FROM Item i 
                             JOIN OrderDetail od ON i.ItemID = od.ItemID 
                             JOIN [Order] o ON o.OrderID = od.OrderID 
                             WHERE o.AgentID = @agentId 
                             GROUP BY i.ItemName";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@agentId", agentId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // 3. Lọc danh sách Đại lý đã mua một sản phẩm cụ thể
        public DataTable GetAgentsByItem(int itemId)
        {
            string query = @"SELECT a.AgentName, SUM(od.Quantity) AS TotalBought 
                             FROM Agent a 
                             JOIN [Order] o ON a.AgentID = o.AgentID 
                             JOIN OrderDetail od ON o.OrderID = od.OrderID 
                             WHERE od.ItemID = @itemId 
                             GROUP BY a.AgentName";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@itemId", itemId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}