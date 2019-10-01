using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace TCPOption
{
    // State object for receiving data from remote device.  
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
    public class AsynchronousClient
    {
        //static Person examplePerson = new Person();
        
        public AsynchronousClient()
        {
            
            StartClient();
        }


        // The port number for the remote device.  
        private const int port = 11573;
        private static Socket client;
        public static Socket Client { get { return client; } }

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        public static ManualResetEvent ConnectDone { get { return connectDone; } }
        public static ManualResetEvent SendDone { get { return sendDone; } }
        public static ManualResetEvent ReceiveDone { get { return receiveDone; } }
        public static string MachineName { get; set; } = "DESKTOP-5896017";

        // The response from the remote device.  
        private static String response = String.Empty;

        public void Close()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private void StartClient()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the   
                // remote device is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(MachineName);
                IPAddress ipAddress = ipHostInfo.AddressList[1];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                client = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP,
                    new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                //// Send test data to the remote device.  
                //Send(client, examplePerson);
                //sendDone.WaitOne();

                //// Receive the response from the remote device.  
                //Receive(client);
                //receiveDone.WaitOne();

                // Write the response to the console.  
                //Console.WriteLine("Response received : {0}", response);

                //// Release the socket.  
                //client.Shutdown(SocketShutdown.Both);
                //client.Close();

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                //Console.WriteLine("Socket connected to {0}",
                    //client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        public void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        //public void SendData<T>(T obj)
        //{
        //    string json = JsonConvert.SerializeObject(obj);

        //    byte[] dataBytes = Encoding.Default.GetBytes(json);
        //    client.Send(dataBytes);
        //    Console.WriteLine("sent...");
        //    byte[] buffer = new byte[1024 * 4];
        //    int readBytes = client.Receive(buffer);
        //    MemoryStream memoryStream = new MemoryStream();
        //    while (readBytes > 0)
        //    {
        //        memoryStream.Write(buffer, 0, readBytes);
        //        if (client.Available > 0)
        //        {
        //            readBytes = client.Receive(buffer);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    Console.WriteLine("read...");
        //    byte[] totalBytes = memoryStream.ToArray();
        //    memoryStream.Close();
        //    string readData = Encoding.Default.GetString(totalBytes);

        //}

        public void Send<T>(Socket client, T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(json);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }
    }
}
