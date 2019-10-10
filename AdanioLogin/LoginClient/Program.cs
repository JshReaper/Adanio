using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace LoginClient
{
    //class Program
    //{
    //    static string user;
    //    static string pass;
        
    //    static void Main(string[] args)
    //    {
    //        LoginSystem.Ip = "localhost";
    //        bool running = true;
    //        while (running)
    //        {
    //            Console.WriteLine("type: \"Signup\", \"Login\" or \"Exit\"");
    //            switch (Console.ReadLine())
    //            {
    //                case "Signup":
    //                    SignUp();
    //                    Console.Clear();
    //                    break;
    //                case "Login":
    //                    AttemptLogin();
    //                    Console.Clear();
    //                    break;
    //                case "Exit":
    //                    running = false;
    //                    Console.Clear();
    //                    break;
    //                default:
    //                    Console.Clear();
    //                    break;
    //            }
    //        }
    //        Console.ReadKey();

    //    }
    //    static void AttemptLogin()
    //    {
    //        Console.Write("Enter credentials:\nUserName: ");
    //        user = Console.ReadLine();
    //        Console.Write("Password: ");
    //        pass = Console.ReadLine();
    //        Console.WriteLine(user + ":" + pass);
    //        KeyValuePair<bool,string> keyValue = LoginSystem.AttemptLogin(user, pass);
            
    //        if (keyValue.Key)
    //        {
    //            Console.WriteLine("logged in");
    //            Console.ReadKey();
    //        }
    //        else
    //        {
    //            Console.Clear();
    //            Console.WriteLine("Invalid login, Please login again");
    //            Console.WriteLine(keyValue.Value);
    //            AttemptLogin();
    //        }

    //    }

    //    static void SignUp()
    //    {
    //        Console.Write("Enter credentials:\nUserName: ");
    //        string user = Console.ReadLine();
    //        Console.Write("Password: ");
    //        string pass = Console.ReadLine();
    //        KeyValuePair<bool, string> keyValue = LoginSystem.AttemptSignup(user, pass);
    //        if (keyValue.Key)
    //        {
    //            Console.WriteLine("User created");
    //        }
    //        else
    //        {
    //            Console.WriteLine(keyValue.Value);
    //        }
    //        Console.ReadKey();
    //    }
        
    //    private static string Get(string user, string pass)
    //    {
    //        string strResponseValue = string.Empty;
    //        HttpWebRequest request = null;
    //        HttpWebResponse response = null;
    //        try
    //        {
    //            //deployed
    //            request = (HttpWebRequest)WebRequest.Create("http://localhost/AdanioLogin/api/values");

    //            //debugging
    //            //request = (HttpWebRequest)WebRequest.Create("http://localhost:58576/api/values");

    //            request.Credentials = new NetworkCredential(user, pass); 
    //            request.Method = "GET";
    //            response = null;
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.Message + ":"+ e.InnerException.Message);
    //        }
            
    //        try
    //        {
    //            response = (HttpWebResponse)request.GetResponse();
    //            //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
    //            using (Stream responseStream = response.GetResponseStream())
    //            {
    //                if (responseStream != null)
    //                {
    //                    using (StreamReader reader = new StreamReader(responseStream))
    //                    {
    //                        strResponseValue = reader.ReadToEnd();
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if(ex.InnerException != null)
    //                strResponseValue = ex.Message.ToString() + ": " + ex.InnerException.Message.ToString();
    //            else
    //            {
    //                strResponseValue = ex.Message.ToString();
    //            }
    //        }
    //        finally
    //        {
    //            if (response != null)
    //            {
    //                ((IDisposable)response).Dispose();
    //            }
    //        }
    //        return strResponseValue;
    //    }

    //    private static string Post(string postData)
    //    {
    //        string responseFromServer;
    //        HttpWebRequest request = null;

    //        //deployed
    //        request = (HttpWebRequest)WebRequest.Create("http://localhost/AdanioLogin/api/values");

    //        //debugging
    //        //request = (HttpWebRequest)WebRequest.Create("http://localhost:58576/api/values");

    //        request.Method = "POST";
    //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
    //        // Set the ContentType property of the WebRequest.
    //        request.ContentType = "application / x - www - form - urlencoded";
    //        // Set the ContentLength property of the WebRequest.
    //        request.ContentLength = byteArray.Length;
    //        // Get the request stream.
    //        Stream dataStream = request.GetRequestStream();
    //        // Write the data to the request stream.
    //        dataStream.Write(byteArray, 0, byteArray.Length);
    //        // Close the Stream object.
    //        dataStream.Close();
    //        try
    //        {
    //            // Get the response.
    //            WebResponse response = request.GetResponse();
    //            // Display the status.
    //            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
    //            // Get the stream containing content returned by the server.
    //            // The using block ensures the stream is automatically closed.
    //            using (dataStream = response.GetResponseStream())
    //            {
    //                // Open the stream using a StreamReader for easy access.
    //                StreamReader reader = new StreamReader(dataStream);
    //                // Read the content.
    //                responseFromServer = reader.ReadToEnd();
    //                // Display the content.
    //                Console.WriteLine(responseFromServer);
    //            }
    //            // Close the response.
    //            response.Close();
    //            return responseFromServer;
    //        }
    //        catch (Exception e)
    //        {
    //            if(e.Message.Contains("(409)"))
    //            {
    //                return "username already exist";
    //            }
    //            return e.Message;
    //        }
    //    }
    //}
}