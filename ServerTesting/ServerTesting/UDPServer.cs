using Nito.AsyncEx;
using Open.Nat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTesting
{
    class UDPServer
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private const int bufSize = 8 * 1024;
        private State state = new State();
        private EndPoint endPointFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;

        private string ip = "192.168.0.100";

        public UDPServer()
        {
            Thread t = new Thread(new ThreadStart(StartServer));
            t.Start();
        }

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        private void StartServer()
        {
            Server(ip.ToString(), 25000);
        }

        private void Server(string address, int port)
        {
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Thread t1 = new Thread(new ThreadStart(Receive));
            t1.Start();
        }

        private void Receive()
        {
            socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref endPointFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socket.EndReceiveFrom(ar, ref endPointFrom);
                socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref endPointFrom, recv, so);
                //Console.WriteLine(String.Format("RECV: {0}: {1}, {2}", endPointFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes)));
            }, state);
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
