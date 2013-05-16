using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using Tibb.Clients;
using System.Windows.Forms;

namespace Tibb.DAO
{
    public class HandlerDB
    {
        /*
         * Constructor
         */
        public HandlerDB(Player player)
        {
            this.player = player;
            initialize();
        }

        /*
         * Inicializa las variables para la conexion.
         */
        private void initialize()
        {
            server = "sql201.260mb.org";
            database = "mb260_11850660_tibb";
            uid = "mb260_11850660";
            password = "said6989601";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + 
		    database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        /*
         * Abre la conexion con la base de datos.
         */
        private bool openConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        /*
         * Cierra la conexion con la base de datos.
         */
        private bool closeConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /*
         * Verifica si el usuario existe.
         */
        public bool existPlayer()
        {
            bool estado = false;
            string query = "SELECT nombre FROM players WHERE nombre='" + player.Name + "'";

            if(this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    estado = true;
                }
            }
            this.closeConnection();
            return estado;
        }

        /*
         * Crea un nuevo registro con los datos del player nuevo.
         */
        public void insertPlayer()
        {
            string name = player.Name;
            string account = player.Account;
            string password = player.Password;
            string server = "";//player.getServerName;
            int level = player.Level;
            System.Threading.Thread.Sleep(200);
            string query = @"INSERT INTO players (nombre, account, password, server, lvl) 
                            VALUES('" + name + "','" + account + "', '" + password + "', '" + server + "', " + level +")";

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.closeConnection();
            }
        }

        /*
         * Actualiza los datos del player.
         */
        public void updatePlayer()
        {
            string query = "UPDATE players SET account='" + player.Account + "', password='" + player.Password + "', account='" + player.ServerName + "', account='" + player.Level + "'  WHERE nombre='" + player.Name + "'";

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.closeConnection();
            }
        }

        /*
         * Borra un registro de la base de datos
         */
        public void deletePlayer()
        {
            string query = "DELETE FROM tableinfo WHERE nombre=''";

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.closeConnection();
            }
        }

        /*
         * Devuelve el numero de entradas en la base de datos.
         */
        public int countData()
        {
            string query = "SELECT Count(*) FROM players";
            int Count = -1;

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                Count = int.Parse(cmd.ExecuteScalar() + "");
                this.closeConnection();
                return Count;
            }
            else
            {
                return Count;
            }
        }

        /*
         *  Retorna la fecha actual.
         */
        public static string MakeMySQLDateTime()
        {
            string MySQL = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            return MySQL;
        }

        private MySqlConnection connection;
        private Player player;
        private string server;
        private string database;
        private string uid;
        private string password;
    }
}