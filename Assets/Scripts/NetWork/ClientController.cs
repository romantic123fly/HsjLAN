using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientController : BaseController
{
    public TcpClient tcpClient;
    protected override void Awake()
    {
        base.Awake();
        tcpClient = GameManager.GetInstance.tcpClient;
        tcpClient.AddEventListener(ProtoType.T_C_HostInfo, SyncHostInfo);
    }

    private void SyncHostInfo(string obj)
    {
        Debug.Log("******接收主机玩家信息******");
        Debug.Log(obj);
        GameManager.GetInstance.SendMessage(ProtoType.T_S_ClientInfo, GameManager.GetInstance.userName);

    }

    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);
        switch (messages.Type)
        {
          
            default:
                break;
        }
    }
}
