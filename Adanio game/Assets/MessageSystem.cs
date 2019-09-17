using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Messaging;
using System;
using TMPro;
using System.Threading.Tasks;
public class MessageSystem : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI messageBox;
    MessageQueue mq;
    Guid id;
    // Start is called before the first frame update
    void Start()
    {
        id = Guid.NewGuid();
        if (!MessageQueue.Exists(".\\private$\\QueueName"))
        {
            MessageQueue.Create(".\\private$\\QueueName");
        }
        mq = new MessageQueue(".\\private$\\QueueName");


        StartCoroutine(Recive());
    }

    // Update is called once per frame
    IEnumerator Recive()
    {
        mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

        string message = "";
        while (message == "")
        {
            try
            {
                Message msg = mq.Peek(TimeSpan.FromSeconds(Time.deltaTime));
                if (msg.Label != id.ToString())
                {

                    message = mq.Receive(TimeSpan.FromSeconds(Time.deltaTime)).Body.ToString();

                }
            }
            catch (Exception)
            {

            }
            yield return null;
        }
        if(message.ToString() != "")
        {
            messageBox.text += "\n" + message;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(Recive());
    }
    public void Send()
    {
        if (inputField.text != "")
        {
            mq.Send(inputField.text,id.ToString());
            messageBox.text += "\n" + inputField.text;
        }


    }
    private void OnApplicationQuit()
    {
        //  MessageQueue.Delete(".\\private$\\QueueName");
    }
}
