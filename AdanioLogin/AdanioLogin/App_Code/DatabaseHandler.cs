using MySql.Data;
using MySql.Data.MySqlClient;
namespace AdanioLogin.App_Code
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
                conn.ChangeDatabase("adaniologin");
            }
            catch (System.Exception e)
            {
                throw  e;
            }
        }
        public void Login()
        {

        }
        
    }
}
