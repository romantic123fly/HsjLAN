using UnityEngine;
using System.Collections;
using SocketPacket;
using System.Collections.Generic;
using System;

public class TcpClient : MonoBehaviour
{
    GameClient _client;
    public  bool isLink;

    public void Awake()
    {
        _client = new GameClient();
        Debug.Log("本地IP ：" + GameTool.GetLocalIP());
    }

    //监听协议
    public void AddEventListener(int MdmNum, MessageListener listener)
    {
        _client.AddEventListener(MdmNum, listener);
    }
    //移除协议
    public void RemoveEventListener(int MdmNum, MessageListener listener)
    {
        if (_client!=null)
        {
            _client.RemoveEventListener(MdmNum, listener);
        }
    }

    //断开连接
    public void DisConnect()
    {
        if (_client != null)
        {
            _client.DisConnect();
        }
    }

    //连接服务器
    public void Connect(string address, int port)
    {
      
        _client.ConnectToServer(address, port);
    }
    //发送协议
    public  void SendMessageToS(int num, string message ="")
    {
        if (_client !=null)
        {
            _client.SendMessage(num, message);
        }
        else
        {
            Debug.LogError("TcpClient空指针。。。");
        }
    } 
    public void Update()
    {
        if (_client != null)
            _client.Update();//接收数据
    }
}
