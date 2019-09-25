using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter 1 to establish a TCP Server Connection or enter 2 for a UDP server connection");
            var input = Console.ReadLine();
            if (input == "1")
            {
                //AsynchronousSocketListener aListener = new AsynchronousSocketListener();
                AsynchronousSocketListener.StartListening();
                Console.ReadLine();
                AsynchronousSocketListener.Close();
            }
            if (input == "2")
            {
                UDPServer serverUDP = new UDPServer();
                Console.ReadLine();
                serverUDP.Close();
            }
            else
            {
                Console.WriteLine("Not a Valid input");
                Console.ReadLine();
            }
        }
    }
}
