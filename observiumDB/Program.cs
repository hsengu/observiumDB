using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace ObserviumDB
{
    class Program
    {
        static void Main(string[] args)
        {
            usrPrompt();
        }

        public static void setDB(DBConnection c)
        {
            ConsoleKeyInfo key;
            String password = "";

            Console.Out.Write("Userid: ");
            c.userId = Console.ReadLine();
            Console.Out.Write("Password: ");
            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            c.password = password;
            c.databaseName = "observium";
            c.server = "rpbusobserv";
        }

        public static void usrPrompt()
        {
            int input;

            do
            {
                Console.Write("\n\nObservium DB Query Tool" +
                            "\n==========================" +
                            "\n1) Query inactive ports (3 month inactivity)" +
                            "\n2) Placeholder" +
                            "\n3) Placeholder" +
                            "\n4) Custom Query" +
                            "\n0) Exit" +
                            "\n\nMake a selection: ");
                input = Convert.ToInt32(Console.ReadLine());
                var c = DBConnection.Instance();

                switch (input)
                {
                    case 1:
                        setDB(c);
                        inactivePorts(c);
                        c.Close();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 0:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Write("Not a valid selection, try again.");
                        break;
                }
            } while (input != 0);
        }
        public static void inactivePorts(DBConnection c)
        {
            int count = 0;
            if (c.IsConnect())
            {
                string query = "SELECT d.device_id,hostname,p.port_label,p.ifLastChange FROM devices AS d JOIN ports AS p ON d.device_id = p.device_id WHERE p.ifOperStatus = 'down' AND p.ifLastChange >= NOW() - INTERVAL 3 MONTH AND UNIX_TIMESTAMP(p.ifLastChange) >= d.last_rebooted + 600;";
                var cmd = new MySqlCommand(query, c.Connection);
                try
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string device_id = reader.GetString(0);
                        string hostname = reader.GetString(1);
                        string port_label = reader.GetString(2);
                        string lastChange = reader.GetString(3);
                        Console.WriteLine(device_id + "," + hostname + "," + port_label + "," + lastChange + ",");
                        count++;
                    }
                    Console.WriteLine("Number of inactive ports: " + count);
                    c.Close();
                }
                catch (Exception e)
                {
                    Console.Out.Write(e.ToString());
                }
            }
        }

        public void outputData()
        {

        }
    }
}
