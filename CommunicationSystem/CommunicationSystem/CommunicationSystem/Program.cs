using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace CommunicationSystem
{ 

    class Program
    {
        static List<Connection> connections = new List<Connection>();
        static string path = @".\PRIVATE$\globalQueue";
        static void Main(string[] args)
        {
            if (!MessageQueue.Exists(path))
            {
                Console.WriteLine("Creating new queue for service");
                MessageQueue.Create(path);
            }

            MessageQueue reciever = new MessageQueue(path);
            reciever.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            reciever.ReceiveCompleted += new ReceiveCompletedEventHandler(MyReceiveCompleted);
            Console.WriteLine("Service is ready to recieve commands");
            reciever.BeginReceive();


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
                string from = content[1];
                string targetDest = content[2];
                string message = content[3];


                switch (cmd)
                {
                    case "NEW":
                        Connection c = new Connection(from,message);
                        connections.Add(c);
                        Console.WriteLine("Received new connection: " + from + ", With name: " + message );
                        break;

                    case "MSG":
                        foreach (var co in connections)
                        {
                            if(co.messageID == targetDest)
                            {
                                //Target exists
                                Message ms = new Message(string.Format("MSG:{0}:{1}:{2}",from,targetDest,message));
                                Console.WriteLine("Received a Message Command from: " + from + ", directed to: " + co.address);
                                mq.Path = @"FormatName:Direct=OS:" + co.address + @"\PRIVATE$\localMessageQueue";
                                mq.Send(ms);
                            }
                        }
                        break;

                    case "TARGETS":
                        Console.WriteLine("A user requested the available connections: " + from);
                        string targets = "";
                        foreach (var co in connections)
                        {
                            if (co.address != from) {
                                targets += co.messageID + ":";
                            }
                        }
                        if(targets.Length > 0)
                            targets = targets.Remove(targets.Length - 1, 1);
                        Message msg = new Message("TARGETS:" + targets);
                        mq.Path = @"FormatName:Direct=OS:" + from + @"\PRIVATE$\localMessageQueue";
                        mq.Send(msg); 
                        break;

                    default:
                        Console.WriteLine("Invalid command recieved from: " + m.SenderId.ToString());
                        break;
                }

                // Restart the asynchronous receive operation.
                mq.Path = path;
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
    [System.Serializable]
    class Connection
    {
        public Connection(string address, string messageID)
        {
            this.address = address;
            this.messageID = messageID;
        }

        public string address;
        public string messageID;
    }
    

}
