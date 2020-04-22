#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             TcpServer
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using SocketPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using YLSocket;

public class TcpServer : MonoBehaviour
{
    Thread serverThread;
    Socket serverSocket;
    Socket scoketClient;
    SocketError socketError;
    SocketCallBack callback = new SocketCallBack();
    private SocketEventDispatcher _dispatcher;
    private PacketProcesser _packetProcesser;
    public  bool isLink;

    public event EventHandler<RecvEvent> RecvEventHandler;
    // Use this for initialization
    void Awake()
    {
        callback.callback += GetClientMessage;
        SocketProcessData process = new SocketProcessData(callback);
        _dispatcher = new SocketEventDispatcher(process);
        _packetProcesser = new PacketProcesser();
        RecvEventHandler += new EventHandler<RecvEvent>(this.OnRecveHandler);

    }
    //开启Tcp服务端
    public void StartSever()
    {
        //定义侦听端口
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Any, 7772);
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(ipEnd);
        serverSocket.Listen(10);

        Debug.Log("Tcp服务器开启，等待连接。。。" + GameTool.GetLocalIP());
        serverThread = new Thread(ServerThread);
        serverThread.IsBackground = true;
        serverThread.Start();
    }
    private void ServerThread()
    {
        //一旦接受连接，创建一个客户端
        scoketClient = serverSocket.Accept();

        IPEndPoint ipEndClient = (IPEndPoint)scoketClient.RemoteEndPoint;
        Debug.Log("客户端链接：" + ipEndClient.Address + ipEndClient.Port);
        while (scoketClient != null)
        {
            Thread.Sleep(100);
            //接收数据
            byte[] recv1 = new byte[1024];
            if (scoketClient.Available <= 0) continue;
            int size = scoketClient.Receive(recv1);
            List<byte> receDate = recv1.ToList();
            byte[] array = receDate.GetRange(0, size).ToArray<byte>();

            if (RecvEventHandler != null)
            {
                RecvEventHandler(this, new RecvEvent(array, size));
            }
        }
    }
    //服务器给链接的客户端发送消息
    public void SendMessageToC(int num, string str = "")
    {
        if (scoketClient == null)
            return;
        byte[] data = new byte[1024];
        data = Encoding.UTF8.GetBytes(num + "|" + str);
        Debug.Log("主机发消息： " + num);
        scoketClient.Send(data, 0, data.Length, SocketFlags.None, out socketError);
    }

    private void OnRecveHandler(object sender, RecvEvent e)
    {
        _dispatcher.AddData(e.Message, e.BytesTransferred);
    }

    /// <summary>
    /// 收到服务器消息
    /// </summary>
    /// <param name="data">一个完整的消息数据包</param>
    private void GetClientMessage(byte[] data)
    {
        var message = Encoding.UTF8.GetString(data);

        _packetProcesser.dispatchEvent(message.Split('|')[0].ToInt(), message.Split('|')[1]);

        Debug.LogError("收到玩家端消息: " + message.Split('|')[0]);
    }

    private void Update()
    {
        _dispatcher.IncomingData();
        if (socketError == SocketError.Success)
        {
            socketError = SocketError.NoData;

        }
        if (scoketClient != null && !scoketClient.Connected)
        {
            Debug.LogError("检测到客户端玩家断开连接");
            scoketClient = null;
        }
    }
    public void AddEventListener(int MdmNum, MessageListener listener)
    {
        _packetProcesser.addEventListener(MdmNum, listener);
    }

    public void RemoveEventListener(int MdmNum, MessageListener listener)
    {
        _packetProcesser.removeEventListener(MdmNum, listener);
    }
    public void DisConnect()
    {
        if (serverSocket != null)
        {
            serverSocket.Close();
            serverSocket = null;
        }
        if (scoketClient != null)
        {
            scoketClient.Close();
            scoketClient = null;
        }

        if (serverThread != null)
        {
            serverThread.Abort();
        }
    }

    private void OnDestroy()
    {
        if (serverThread != null)
        {
            serverThread.Abort();
        }
        if (serverSocket != null)
        {
            serverSocket.Close();
            serverSocket = null;
        }
    }
}
