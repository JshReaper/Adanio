using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LoginClient
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User("dummy", "dumdum");
            Console.Write( Get(user));
            Console.ReadKey();
            System.Net.Http.Headers.
        }

        private static string Get(User user)
        {
            MemoryStream memorystream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(memorystream, user);
            byte[] userBytes = memorystream.ToArray();

            string userStr = "";

            foreach (var b in userBytes)
            {
                userStr += b;
            }
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:49770/api/Values?userStr=" + userStr);
            request.Method = "GET";
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            return strResponseValue;
        }
    }
    [Serializable]
    public class User
    {
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }


}
