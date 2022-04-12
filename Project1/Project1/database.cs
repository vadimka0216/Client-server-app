using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Project1
{
    class database
    {
        const string serverName = "DESKTOP-IJ5BCUF\\SQLEXPRESS";//localhost
        string settingsConnect = "";
        SqlConnection connection;
        public database(string nameDB, string login=null, string password=null)
        {
            if (login==null&&password==null)
                settingsConnect = "Data Source='"+serverName+"';Initial Catalog='"+ nameDB+"';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            else
                settingsConnect = "Data Source='" + serverName + "';Initial Catalog='" + nameDB + "';User ID='" + login + "'; Password='" + password + "';Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            if (!tryConnect())
                connection = null;
        }

        private bool tryConnect()
        {
            if (isOpen()) return false;

            bool errCon = false;
            try
            {
                connection = new SqlConnection(settingsConnect);
                connection.Open();
            }
            catch
            {
                errCon = true;
            }

            if (!errCon&&connection.State == ConnectionState.Closed)
            {
                errCon = true;
                MessageBox.Show("Ошибка подключения!");
            }

            return !errCon;
        }

        public void CloseConnect()
        {
            if (isOpen())
            {
                //MessageBox.Show(connection.State+"");
               // try
                //{
                    connection.Dispose();
                    connection = null;
                    //connection.Close();
                //}
                //catch { };
            }
        }

        public bool isOpen()
        {
            bool result = false;
            if (connection != null && connection.State != ConnectionState.Closed)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            //return (connection != null && connection.State != ConnectionState.Closed);
            return result;
        }

        public void Query(string command)
        {
            if (isOpen())
            {
                try
                {
                    SqlDataReader sqlReader = (new SqlCommand(command, connection)).ExecuteReader();
                    sqlReader.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Произошло исключение в выполнении запроса \"" + command + "\"\r\nСообщение: " + exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public List<string> ReadOrderData(string command)
        {
            List<string> result = new List<string>();
            if (isOpen())
            {
                SqlDataReader reader = (new SqlCommand(command, connection)).ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++ )
                        result.Add(reader[i].ToString());
                }

                // Call Close when done reading.
                reader.Close();
            }
            return result;
        }

        ~database()
        {
            this.CloseConnect();
        }
    }
}
