using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace ServerTesting
{
    class TCPServer
    {
        private string ip = "192.168.0.100";
        Socket socket;
        TcpListener myListener;
        public byte[] ByteArray { get; private set; }

        public TCPServer()
        {
            Thread t = new Thread(new ThreadStart(TCPServerStart));
            t.Start();
        }

        public void TCPServerStart()
        {
            int port = 50000;
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip);


                myListener = new TcpListener(ipAddress, port);

                myListener.Start();

                //Console.WriteLine("The is running at port " + port.ToString());
                //Console.WriteLine("The Local Endpoint is : " + myListener.LocalEndpoint);

                Thread t1 = new Thread(new ThreadStart(WaitAndAccept));
                t1.Start();
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error..." + e);
            }
        }

        private void WaitAndAccept()
        {
            //Console.WriteLine("Waiting for a connection...");
            socket = myListener.AcceptSocket();
            //Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);
            Thread t2 = new Thread(new ThreadStart(ServerReceive));
            t2.Start();
        }

        public void ServerReceive()
        {
            ByteArray = new byte[1024];
            int k = socket.Receive(ByteArray);
            //Console.WriteLine("Recieved...");
            //for (int i = 0; i < k; i++)
                //Console.Write(Convert.ToChar(b[i]));

            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            socket.Send(asciiEncoding.GetBytes("The Object was received by the server."));
            //Console.WriteLine("Sent Acknowledgement");
        }

        public void StopServer()
        {
            socket.Close();
            myListener.Stop();
        }
    }
}
