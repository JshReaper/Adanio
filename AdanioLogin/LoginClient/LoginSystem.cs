using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LoginClient
{
    public static class LoginSystem
    {
        public static string Ip { get; set; }
        public static KeyValuePair<bool,string> AttemptLogin(string user, string pass)
        {
            string result = Get(user, pass);
            if (result == "[\"value1\",\"value2\"]")
            {
                return new KeyValuePair<bool, string>(true,result);
            }
            else
            {
                return new KeyValuePair<bool, string>(false, result);
            }
        }
        /// <summary>
        /// retuns true if user is created
        /// returns false with an error message if not
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static KeyValuePair<bool,string> AttemptSignup(string user, string pass)
        {
            KeyValuePair<bool, string> keyValue;
            string result = Post(user + ":" + pass);
            if(result == "Created")
            {
                keyValue = keyValue = new KeyValuePair<bool, string>(true,result);
                return keyValue;
            }
            else
            {
                keyValue = keyValue = new KeyValuePair<bool, string>(false, result);
                return keyValue;
            }
        }
        private static string Get(string user, string pass)
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //deployed
                request = (HttpWebRequest)WebRequest.Create("http://" + Ip + "/AdanioLogin/api/values");

                //debugging
                //request = (HttpWebRequest)WebRequest.Create("http://localhost:58576/api/values");

                request.Credentials = new NetworkCredential(user, pass);
                request.Method = "GET";
                response = null;
            }
            catch (Exception e)
            {
                return e.Message;
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
                if (ex.InnerException != null)
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

        private static string Post(string postData)
        {
            string responseFromServer;
            HttpWebRequest request = null;

            //deployed
            request = (HttpWebRequest)WebRequest.Create("http://" + Ip + "/AdanioLogin/api/values");

            //debugging
            //request = (HttpWebRequest)WebRequest.Create("http://localhost:58576/api/values");

            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application / x - www - form - urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            try
            {
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    if(responseFromServer == "")
                    responseFromServer = ((HttpWebResponse)response).StatusDescription;
                }
                // Close the response.
                response.Close();
                return responseFromServer;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("(409)"))
                {
                    return "username already exist";
                }
                return e.Message;
            }
        }
    }
}
