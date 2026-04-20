using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ItemDAL : DBConnection
    {
        // Lấy danh sách sản phẩm
        public DataTable GetAllItems()
        {
            string query = "SELECT * FROM Item";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // Thêm sản phẩm
        public bool InsertItem(ItemDTO item)
        {
            string query = "INSERT INTO Item (ItemName, Size, Price) VALUES (@name, @size, @price)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", item.ItemName);
            cmd.Parameters.AddWithValue("@size", item.Size);
            cmd.Parameters.AddWithValue("@price", item.Price);

            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();
            return result > 0;
        }

        // Lấy giá theo ItemID (có thể trả về null nếu cột Price không tồn tại hoặc giá là NULL)
        public double? GetPriceById(int itemId)
        {
            string query = "SELECT Price FROM Item WHERE ItemID = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", itemId);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return null;
                return Convert.ToDouble(result);
            }
            finally
            {
                conn.Close();
            }
        }

        // Các hàm UpdateItem và DeleteItem tương tự với lệnh UPDATE và DELETE
    }
}