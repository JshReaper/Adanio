using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCPOption;

public class AsyncTCP : MonoBehaviour
{
    private AsynchronousSocketListener aListener;
    private AsynchronousClient aClient;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateSocketExample()
    {
        aListener = new AsynchronousSocketListener();
    }

    public void CreateClientExample()
    {
        aClient = new AsynchronousClient();

        aClient.Send<string>(AsynchronousClient.Client, "Test message");
        AsynchronousClient.SendDone.WaitOne();

        aClient.Receive(AsynchronousClient.Client);
        AsynchronousClient.ReceiveDone.WaitOne();
        Debug.Log("Done");
    }

    private void OnApplicationQuit()
    {
        if (aListener != null)
            aListener.Close();

        if (aClient != null)
            aClient.Close();
    }
}
