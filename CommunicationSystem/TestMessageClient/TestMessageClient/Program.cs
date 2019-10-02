using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace TestMessageClient
{
    class Program
    {
        static string serverPath = @"FormatName:Direct=TCP:192.168.0.104\PRIVATE$\globalQueue";
        static string localQueue = @".\PRIVATE$\localMessageQueue";

        static void Main(string[] args)
        {

            if (!MessageQueue.Exists(".\\PRIVATE$\\localMessageQueue"))
            {
                MessageQueue.Create(".\\PRIVATE$\\localMessageQueue");
            }


            MessageQueue mq = new MessageQueue(localQueue);

            string myLocalName = System.Environment.MachineName;
            Console.WriteLine(myLocalName);
            mq.Path = serverPath;
            Console.ReadKey();

            //Command:From:Target:Message
            Message m = new Message("NEW:" + myLocalName + ":blops:" + "Topps");
            mq.Send(m);

            Console.WriteLine("Sent message: " + m.Body.ToString());

            mq.Path = localQueue;
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            mq.ReceiveCompleted += new ReceiveCompletedEventHandler(MyReceiveCompleted);
            mq.BeginReceive();

            Console.ReadLine();
        }




        private static void MyReceiveCompleted(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                // Connect to the queue.
                MessageQueue mq = (MessageQueue)source;

                // End the asynchronous receive operation.
                Message m = mq.EndReceive(asyncResult.AsyncResult);


                //Command:From:Target:Message
                string[] content = m.Body.ToString().Split(':');
                string cmd = content[0];
                switch (cmd)
                {
                    case "TARGETS":
                        Console.WriteLine("The following messageID's exists on service list:");
                        for (int i = 1; i < content.Length; i++)
                        {
                            Console.WriteLine("#" + content[i]);
                        }
                        break;
                    case "MSG":

                        string from = content[1];
                        string message = content[3];

                        Console.WriteLine("@" + from + ": " + message);
                        break;

                    default:
                        break;
                }
                // Restart the asynchronous receive operation.
                mq.Path = localQueue;
                mq.BeginReceive();
            }
            catch (MessageQueueException e)
            {
                // Handle sources of MessageQueueException.
                Console.WriteLine("ERROR: " + e);
            }

            // Handle other exceptions.

            return;
        }

    }
}
