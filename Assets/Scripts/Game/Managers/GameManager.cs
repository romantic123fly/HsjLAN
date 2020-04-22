#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             GameManager
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
public class GameManager : BaseManager<GameManager>
{
    public string version = "1.0.1";
    public string vrVersion = "";
    public string localIP;//本地IP
    public bool isClient;
    public bool isServer;
    public string CurHostIP { get; set; }//d当前主机ip
    public Dictionary<string, string[]> deviceList; //udp 接收到的设备列表，dp/id

    public TcpClient tcpClient;
    public TcpServer tcpServer;
    public bool tcpDisConnect;
    public string userName;
    public override void SetUp()
    {
        base.SetUp();
        localIP = GameTool.GetLocalIP();
        userName = localIP;
        Debug.Log("本地IP ：" + localIP);
        deviceList = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// 核实本地bindid跟服务器的bindid是否一致
    /// </summary>
    /// <param name="data">服务器的BindId</param>
    public void StartUdpCheck()
    {
        //TODO 开启udp广播等待连接
        UdpManager.GetInstance.SendBroadcast();
        tcpServer = gameObject.AddComponent<TcpServer>();
        tcpServer.StartSever();
        GameObject.Find("Controller").gameObject.AddComponent<ServerController>();
    }

    protected override void Update()
    {
        base.Update();
        if (tcpDisConnect)
        {
            Debug.LogError("断开连接");
            //如果检测到VR端Tcpserver断开连接，就返回主界面
            deviceList.Clear();
            if (tcpClient != null)
            {
                tcpClient.DisConnect();
                tcpClient = null;
            }
            MessageDispatcher.Dispatch(MessagesType.ShowMainView);
            UdpManager.GetInstance.ListenMes();
            tcpDisConnect = false;
        }

        //退出应用
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
 
    public IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public void SendMessage(int protoType, string str)
    {
        if (isServer)
        {
            tcpServer.SendMessageToC(protoType, str);
        }
        else
        {
            tcpClient.SendMessageToS(protoType, str);
        }
    }
}

