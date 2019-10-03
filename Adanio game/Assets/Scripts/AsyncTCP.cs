﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCPOption;

public class AsyncTCP : MonoBehaviour
{
    private AsynchronousSocketListener aListener;
    private AsynchronousClient aClient;

    [SerializeField]
    private string ipAdress;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(aListener != null && aListener.MessageSent == true)
        {
            Debug.Log(aListener.DebugMessage);
            aListener.MessageSent = false;
        }
    }

    public void CreateSocketExample()
    {
        aListener = new AsynchronousSocketListener();
    }

    public void Send()
    {
        string message = "Hello, World!";
        aClient.Send<string>(AsynchronousClient.Client, message);
        AsynchronousClient.SendDone.WaitOne();

        aClient.Receive(AsynchronousClient.Client);
        AsynchronousClient.ReceiveDone.WaitOne();
        Debug.Log(message);
    }

    public void CreateClientExample()
    {
        aClient.ipAddress = ipAdress;

        aClient = new AsynchronousClient();

        
    }

    private void OnApplicationQuit()
    {
        if (aListener != null)
            aListener.Close();

        if (aClient != null)
            aClient.Close();
    }
}
