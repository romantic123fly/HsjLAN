using UnityEngine;
using System;
using YLSocket;
using SocketPacket;
using System.Collections.Generic;
using System.Text;

public class GameClient
{
    public  SocketClient _client = null;
    private SocketEventDispatcher _dispatcher;

    private PacketProcesser _packetProcesser;
    public GameClient()
    {
        InitClient();
    }

    private void OnConnectHandler(object sender, ConnectEvent e)
    {
        Debug.Log("链接Tcp服务器成功");
        //GameManager.GetInstance.tcpClient.SendMeToServer(ProtoType.T_S_UniqueID, GameManager.GetInstance.selfUniqueIdentifier);
    }
    private void OnCloseHandler(object sender, EventArgs e)
    {
        Debug.Log("链接关闭" );
        GameManager.GetInstance.tcpDisConnect = true;

    }

    private void OnConnectErrorHandler(object sender, EventArgs e)
    {
        Debug.Log("链接错误");

    }
    private void OnReconnectHandler(object sender, EventArgs e)
    {
        Debug.Log("链接断开");
        GameManager.GetInstance.tcpDisConnect = true;
     
    }

    public void Update()
    {
        _dispatcher.IncomingData();
    }

    public void OnRecveHandler(object sender, RecvEvent e)
    {
        _dispatcher.AddData(e.Message, e.BytesTransferred);
    }
    public void InitClient()
    {
        _packetProcesser = new PacketProcesser();

        SocketCallBack callback = new SocketCallBack();
        callback.callback += GetServerMessage;
        SocketProcessData process = new SocketProcessData(callback);
        _dispatcher = new SocketEventDispatcher(process);


        _client = new SocketClient();
        _client.RecvEventHandler += new EventHandler<RecvEvent>(this.OnRecveHandler);
        _client.ConnectHandler += new EventHandler<ConnectEvent>(this.OnConnectHandler);
        _client.CloseHandler += new EventHandler(this.OnCloseHandler);
        _client.ConnectErrorHandler += new EventHandler(this.OnConnectErrorHandler);
        _client.ReconnectHandler += new EventHandler(this.OnReconnectHandler);
    }

    public void ConnectToServer(string ip, int port)
    {
        _client.CreateConnection(ip, port);
    }

    public void DisConnect()
    {
        _client.DisConnect();
    }

    /// <summary>
    /// 收到服务器消息
    /// </summary>
    /// <param name="data">一个完整的消息数据包</param>
    private void GetServerMessage(byte[] data)
    {
        var message = Encoding.UTF8.GetString(data);

        _packetProcesser.dispatchEvent(message.Split('|')[0].ToInt(), message.Split('|')[1]);
    }

    /// <summary>
    /// 消息对象发给服务器
    /// </summary>
    /// <param name="messageObject"></param>
    public void SendMessage( int num, string messageByte)
    {
        var date= Encoding.UTF8.GetBytes(num +"|"+messageByte);
        _client.SendMessageToServer(date);
        Debug.Log("Pad发消息： "+num);
    }

    public void AddEventListener(int MdmNum,  MessageListener listener)
    {
        _packetProcesser.addEventListener( MdmNum,listener);
    }

    public void RemoveEventListener(int MdmNum, MessageListener listener)
    {
        _packetProcesser.removeEventListener(MdmNum, listener);
    }

}