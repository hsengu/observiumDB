using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ObserviumDB
{
    public class DBConnection
    {
        private DBConnection()
        {
        }

        public string server { get; set; }
        public string databaseName { get; set; }
        public string userId { get; set; }
        public string password { get; set; }

        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
        
            _instance = new DBConnection();
    
            return _instance;

        }

        public bool IsConnect()
        {
            //if (Connection == null)
            //{
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
                conn_string.Server = server;
                conn_string.Port = 3306;
                conn_string.UserID = userId;
                conn_string.Password = password;
                conn_string.Database = databaseName;
                conn_string.SslMode = new MySqlSslMode();
                connection = new MySqlConnection(conn_string.ToString());
                try
                {
                    connection.Open();
                }
                catch (MySqlException e)
                {
                    Console.Out.Write(e.ToString());
                }
            //}
            return true;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
