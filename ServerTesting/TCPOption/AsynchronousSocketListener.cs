using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace TCPOption
{
    public class AsynchronousSocketListener
    {
        // Thread signal.  
        IPHostEntry ipHostInfo;
        IPAddress ipAddress;
        IPEndPoint localEndPoint;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static Socket listener;
        public static Socket Listener { get { return listener; } }
        public bool EndConnection { get; set; } = false;

        public string DebugMessage { get; private set; }
        public bool MessageSent { get; set; } = false;
        public List<Socket> ConnectedClients { get; private set; } = new List<Socket>();
        

        public AsynchronousSocketListener()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[1];
            localEndPoint = new IPEndPoint(ipAddress, 11573);

            // Create a TCP/IP socket.  
            listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            Thread t = new Thread(new ThreadStart(StartListening));
            t.Start();
        }

        public void Close()
        {
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }

        public void StartListening()
        {
            int iterations = 0;
            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    
                    // Set the event to nonsignaled state.  
                    allDone.Reset();
                    if (EndConnection)
                    {
                        break;
                    }
                    // Start an asynchronous socket to listen for connections.  
                    //Console.WriteLine("Waiting for a connection...");
                    
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    iterations++;

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
            finally
            {
                Close();
            }

            //Console.WriteLine("\nPress ENTER to continue...");
            //Console.Read();

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();
            

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            ConnectedClients.Add(handler);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                //if (content.IndexOf("<EOF>") > -1)
                //{
                // All the data has been read from the   
                // client. Display it on the console.  
                //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                //content.Length, content);
                // Echo the data back to the client.  
                Send(handler, content);
                //}
                //else
                //{
                //    // Not all data received. Get more.  
                //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReadCallback), state);
                //}
            }
        }

        public void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                DebugMessage = "Message Received and sent";
                MessageSent = true;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

    }
}
