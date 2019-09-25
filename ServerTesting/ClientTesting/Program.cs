using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter 1 to establish a Connection to a TCP Server or enter 2 for a UDP server connection");
            string input = Console.ReadLine();
            if (input == "1")
            {
                AsynchronousClient aClient = new AsynchronousClient();
                Console.ReadLine();
                aClient.Close();
            }
            if (input == "2")
            {
                UDPClient clientUDP = new UDPClient();
                Console.ReadLine();
                clientUDP.Close();
            }
        }
    }
}
