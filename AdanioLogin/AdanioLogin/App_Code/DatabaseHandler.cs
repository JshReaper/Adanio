using MySql.Data;

namespace AdanioLogin.App_Code
{
    public class DatabaseHandler
    {
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

        }
        public void Login()
        {

        }
    }
}
