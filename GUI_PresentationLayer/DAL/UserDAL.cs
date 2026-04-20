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
    public class UserDAL : DBConnection
    {
        public bool CheckLogin(UserDTO user)
        {
            string query = "SELECT * FROM Users WHERE UserName = @username AND Password = @password AND [Lock] = 0";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", user.UserName);
            cmd.Parameters.AddWithValue("@password", user.Password);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            bool hasRows = reader.HasRows;
            conn.Close();

            return hasRows;
        }
    }
}