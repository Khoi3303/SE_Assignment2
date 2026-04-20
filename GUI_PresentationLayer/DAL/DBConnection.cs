using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DBConnection
    {
        protected SqlConnection conn;

        public DBConnection()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            conn = new SqlConnection(connString);
        }
    }
}