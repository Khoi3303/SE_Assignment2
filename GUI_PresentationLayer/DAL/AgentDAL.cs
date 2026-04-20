using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class AgentDAL : DBConnection
    {
        // Hàm lấy toàn bộ danh sách Đại lý từ cơ sở dữ liệu
        public DataTable GetAllAgents()
        {
            string query = "SELECT * FROM Agent";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}