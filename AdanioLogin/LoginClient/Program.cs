using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace LoginClient
{
    class Program
    {
        static void Main(string[] args)
        {
            AttemptLogin();

            Console.ReadKey();

        }
        static void AttemptLogin()
        {
            Console.Write("Enter credentials:\nUserName: ");
            user = Console.ReadLine();
            Console.Write("Password: ");
            pass = Console.ReadLine();
            Console.WriteLine(user + ":" + pass);
            string result = Get();
            if (result == "The remote server returned an error: (401) Unauthorized.")
            {
                Console.Clear();
                Console.WriteLine("Invalid login, Please login again");
                AttemptLogin();
            }
            else
            {
                Console.WriteLine(result);
            }
        }
        static string user;
        static string pass;
        private static string Get()
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //deployed
                //request = (HttpWebRequest)WebRequest.Create("http://localhost/AdanioLogin/api/values");

                //debugging
                request = (HttpWebRequest)WebRequest.Create("http://localhost:58576/api/values");

                request.Credentials = new NetworkCredential(user, pass); 
                request.Method = "GET";
                response = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ":"+ e.InnerException.Message);
            }
            
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
                if(ex.InnerException != null)
                    strResponseValue = ex.Message.ToString() + ": " + ex.InnerException.Message.ToString();
                else
                {
                    strResponseValue = ex.Message.ToString();
                }
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
}
       
