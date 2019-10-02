using System.Collections;
using System.Collections.Generic;
using System.Messaging;
using UnityEngine;

public class ChatClient : MonoBehaviour
{
    static string serverPath = @"FormatName:Direct=TCP:192.168.0.104\PRIVATE$\globalQueue";
    static string localQueue = @".\PRIVATE$\localMessageQueue";
    string myLocalName = System.Environment.MachineName;
    [SerializeField] Transform messageIdUi;
    [SerializeField] Transform parrentUi;

    TextBoxSubmitter tbs;

    MessageQueue mq = new MessageQueue(localQueue);
    // Start is called before the first frame update
    void Start()
    {
        if (!MessageQueue.Exists(".\\PRIVATE$\\localMessageQueue"))
        {
            MessageQueue.Create(".\\PRIVATE$\\localMessageQueue");
        }
        mq.Path = serverPath;

        Message m = new Message("NEW:" + myLocalName + ":blops:" + "Topps");
        mq.Send(m);

        tbs = FindObjectOfType<TextBoxSubmitter>();
        StartCoroutine(Recieve());
        RequestTargets();

    }


    public void Send(string cmd, string target, string message)
    {
        if (target.Contains(":"))
        {
            string[] targets = target.Split(':');

            target = "";
            foreach (var t in targets)
            {
                target += t + "#";
            }
            target = target.Remove(target.Length-1,1);
        }


        string toSend = cmd + ":" + myLocalName + ":" + target + ":" + message;

        mq.Path = serverPath;
        mq.Send(new Message(toSend));

        mq.Path = localQueue;
    }
    
    public void RequestTargets()
    {
        mq.Path = serverPath;
        mq.Send(new Message("TARGETS:" + myLocalName + "null" + "null"));

        mq.Path = localQueue;
    }


    private IEnumerator Recieve()
    {
        yield return null;
        mq.Path = localQueue;
        mq.Formatter = new XmlMessageFormatter(new System.Type[] { typeof(string) });
        mq.ReceiveCompleted += new ReceiveCompletedEventHandler(MyReceiveCompleted);
        mq.BeginReceive();
    }


    private void MyReceiveCompleted(System.Object source, ReceiveCompletedEventArgs asyncResult)
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
                    Debug.Log("The following messageID's exists on service list:");

                    foreach (Transform child in parrentUi)
                    {
                        Destroy(child.gameObject);
                    }
                    for (int i = 1; i < content.Length; i++)
                    {
                        Instantiate(messageIdUi, parrentUi).GetComponentInChildren<TMPro.TextMeshProUGUI>().text = content[i];
                    }
                    break;
                case "MSG":

                    string from = content[1];
                    string message = content[3];


                    if (tbs)
                    {
                        tbs.IncomingMessage(from + ": " + message);
                    }

                    Debug.Log("@" + from + ": " + message);
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
            Debug.Log("ERROR: " + e);
        }

        // Handle other exceptions.

        return;
    }

}
