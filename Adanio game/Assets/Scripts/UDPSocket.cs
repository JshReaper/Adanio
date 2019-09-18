using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPSocket : MonoBehaviour
{
    private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    private const int bufSize = 8 * 1024;
    private State state = new State();
    private EndPoint endPointFrom = new IPEndPoint(IPAddress.Any, 0);
    private AsyncCallback recv = null;
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
        Server("127.0.0.1", 25000);
    }

    public void StartClient()
    {
        Client("127.0.0.1", 25000);
        Send("TEST!");
    }

    public class State
    {
        public byte[] buffer = new byte[bufSize];
    }

    public void Server(string address, int port)
    {
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
        socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
        Receive();
    }

    public void Client(string address, int port)
    {
        socket.Connect(IPAddress.Parse(address), port);
        Receive();
    }

    public void Send(string text)
    {
        byte[] data = Encoding.ASCII.GetBytes(text);
        socket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = socket.EndSend(ar);
            Debug.Log(string.Format("SEND: {0}, {1}", bytes, text));
        }, state);
    }

    private void Receive()
    {
        socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref endPointFrom, recv = (ar) =>
        {
            State so = (State)ar.AsyncState;
            int bytes = socket.EndReceiveFrom(ar, ref endPointFrom);
            socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref endPointFrom, recv, so);
            Debug.Log(String.Format("RECV: {0}: {1}, {2}", endPointFrom.ToString(), bytes, Encoding.ASCII.GetString(so.buffer, 0, bytes)));
        }, state);
    }
}

