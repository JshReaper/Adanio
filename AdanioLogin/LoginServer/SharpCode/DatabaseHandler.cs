using MySql.Data.MySqlClient;

namespace LoginServer.SharpCode
{
    public class DatabaseHandler
    {
        MySqlConnection conn;

        private static DatabaseHandler instance;
        public static DatabaseHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseHandler();
                }
                return instance;
            }
        }
        private DatabaseHandler()
        {
            string connStr = "server=localhost;user=root;port=3306;password=t0xuWgHPHq4%";
            conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string sql = "CREATE DATABASE IF NOT EXISTS adaniologin";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                }
                rdr.Close();
                conn.ChangeDatabase("adaniologin");
                sql = "CREATE TABLE IF NOT EXISTS adaniologin.users (id INT NOT NULL PRIMARY KEY AUTO_INCREMENT," +
                    "username VARCHAR(50) NOT NULL UNIQUE," +
                    "password VARCHAR(255) NOT NULL," +
                    "created_at DATETIME DEFAULT CURRENT_TIMESTAMP);";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                }
                rdr.Close();

                sql = "SELECT * FROM adaniologin.users";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();

                    sql = "INSERT INTO users (username, password) VALUES ('dummy', 'dumdum')";
                    cmd = new MySqlCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                    }
                    rdr.Close();
                }
                else
                {
                    rdr.Close();
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        public bool Login(string username, string password)
        {
            string sql = "SELECT * FROM adaniologin.users WHERE username = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr.HasRows)
                {
                    if (rdr[2].ToString() == password)
                    {
                        rdr.Close();
                        return true;
                    }
                }
                else
                {
                    rdr.Close();
                    return false;
                }
            }
            rdr.Close();
            return false;
        }

    }
}