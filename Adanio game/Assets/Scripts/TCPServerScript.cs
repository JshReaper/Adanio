using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCPServerScript : MonoBehaviour
{
    [SerializeField]
    string ip;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartServer()
    {
        int port = 50000;
        try
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            

            TcpListener myListener = new TcpListener(ipAddress, port);

            myListener.Start();

            Debug.Log("The is running at port " + port.ToString());
            Debug.Log("The Local Endpoint is : " + myListener.LocalEndpoint);
            Debug.Log("Waiting for a connection...");

            Socket socket = myListener.AcceptSocket();
            Debug.Log("Connection accepted from " + socket.RemoteEndPoint);

            byte[] b = new byte[100];
            int k = socket.Receive(b);
            Debug.Log("Recieved...");
            for (int i = 0; i < k; i++)
                Debug.Log(Convert.ToChar(b[i]));

            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            socket.Send(asciiEncoding.GetBytes("The string was received by the server."));
            Debug.Log("Sent Acknowledgement");

            socket.Close();
            myListener.Stop();
        }
        catch(Exception e)
        {
            Debug.Log("Error..." + e);
        }
    }
}
