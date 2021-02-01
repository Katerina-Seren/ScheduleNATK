using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Schedule
{
    class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "91.227.68.11";
            int port = 3306;
            string database = "pl4453-mobile";
            string username = "das";
            string password = "0B1a1J2c";


            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }

    }
}