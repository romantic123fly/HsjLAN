#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             UdpManager
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpManager : BaseManager<UdpManager> {

    // 用于UDP发送的网络服务类
    private UdpClient udpcSend;
    // 用于UDP接收的网络服务类
    private UdpClient udpcRecv;
    // 线程：发送UDP报文
    Thread thrSend;
    // 线程：不断监听UDP报文
    Thread thrRecv;
    //是否发送消息广播
    private bool isSendMes = true;

    //Vr端(主机)——广播数据
    public void SendBroadcast()
    {
        Debug.Log("开始广播消息");
        IPEndPoint localIpep = new IPEndPoint(IPAddress.Any, 7772); // 自动分配本机IP，指定的端口号
        udpcSend = new UdpClient(localIpep);
        thrSend = new Thread(SendMessage);
        thrSend.IsBackground = true;
        thrSend.Start();
    }

    private void SendMessage()
    {
        while (isSendMes)
        {
            Thread.Sleep(100);
            if (udpcSend == null) break;
            
            Debug.Log(" 我是主机我在发送广播");
            byte[] sendbytes = Encoding.Unicode.GetBytes("我是主机我在发送广播");
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Broadcast, 7777); // 发送到的IP地址和端口号
            udpcSend.Send(sendbytes, sendbytes.Length, remoteIpep);
        }
    }

    //Pad端(客户端)——监听数据
    public void ListenMes()
    {
        if (thrRecv != null && thrRecv.IsAlive)
        {
            Debug.Log("udp客户端已经启动请勿重复操作");
            return;
        }
      
        IPEndPoint localIpep = new IPEndPoint(IPAddress.Any, 7777); // 本机IP和监听端口号
        udpcRecv = new UdpClient(localIpep);
        thrRecv = new Thread(ReceiveMessage);
        //thrRecv.IsBackground = true;
        thrRecv.Start();
        Debug.Log("Pad开始监听： "+ GameManager.GetInstance.localIP);
    }
    private void ReceiveMessage(object obj)
    {
        IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 7777);

        while (true)
        {
            if (udpcRecv == null) return;

            if (udpcRecv.Available <= 0) continue;//不做判断会异常：一个封锁操作被对 WSACancelBlockingCall 的调用中断
            byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
            string message = Encoding.Unicode.GetString(bytRecv, 0, bytRecv.Length);
            if (GameManager.GetInstance.deviceList.ContainsKey(remoteIpep.Address.ToString())) continue;

            //加入设备列表
            Debug.Log("监听到：" + remoteIpep.ToString() + "|" + message);
            if (!GameManager.GetInstance.deviceList.ContainsKey(remoteIpep.Address.ToString()))
            {
                var mes = message.Split('|');
                GameManager.GetInstance.deviceList.Add(remoteIpep.Address + "", mes);
            }
        }
    }

    public void CloseUdp()
    {
        if (thrSend != null && thrSend.IsAlive)
            thrSend.Abort();
        if (thrRecv != null && thrRecv.IsAlive)
            thrRecv.Abort();
        if (udpcRecv != null)
        {
            udpcRecv.Close();
            udpcRecv = null;
        }
        if (udpcSend!=null)
        {
            udpcSend.Close();
            udpcSend = null;
        }
       
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CloseUdp();
    }
}
