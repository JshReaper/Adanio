using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class TCPClientScript : MonoBehaviour
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

    public void StartClient()
    {
        int port = 50000;
        try
        {
            TcpClient tcpCliant = new TcpClient();
            Debug.Log("Connecting... ");

            tcpCliant.Connect(ip, port);

            Debug.Log("Connected");

            String message = "Hello, World!";
            Stream stream = tcpCliant.GetStream();

            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] ba = asciiEncoding.GetBytes(message);
            Debug.Log("Transmitting");

            stream.Write(ba, 0, ba.Length);

            byte[] ba2 = new byte[100];
            int k = stream.Read(ba2, 0, 100);

            for (int i = 0; i < k; i++)
                Debug.Log(Convert.ToChar(ba2[i]));

            tcpCliant.Close();

        }
        catch(Exception e)
        {
            Debug.Log("Error... " + e.Message);
        }
    }
}
