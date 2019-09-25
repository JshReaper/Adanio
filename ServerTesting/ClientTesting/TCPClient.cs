using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace ClientTesting
{
    class TCPClient
    {
        private string ip = "192.168.0.100";
        TcpClient tcpCliant;
        Stream stream;
        int port = 50000;

        public TCPClient()
        {
            Thread t = new Thread(new ThreadStart(StartClient));
            t.Start();
        }
        public void StartClient()
        {
            
            try
            {
                tcpCliant = new TcpClient();

                Thread t1 = new Thread(new ThreadStart(Connect));
                t1.Start();

               
            }
            catch (Exception e)
            {
                //Console.WriteLine("Error... " + e.Message);
            }
        }

        public void SendData<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            stream = tcpCliant.GetStream();

            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] ba = asciiEncoding.GetBytes(json + "<EOF>");
            //Console.WriteLine("Transmitting");

            stream.Write(ba, 0, ba.Length);

            byte[] ba2 = new byte[100];
            int k = stream.Read(ba2, 0, 100);

            //for (int i = 0; i < k; i++)
            //    Console.Write(Convert.ToChar(ba2[i]));
        }

        private void SendExample()
        {
            Item item = new Item();
            item.Name = "Message";
            item.Date = DateTime.Now;
            SendData<Item>(item);
        }

        private void Connect()
        {
            //Console.WriteLine("Connecting... ");
            tcpCliant.Connect(ip, port);
            //Console.WriteLine("Connected");
        }

        public void CloseClient()
        {
            tcpCliant.Close();
        }
    }
}
