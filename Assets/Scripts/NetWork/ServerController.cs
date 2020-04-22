using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
public class ServerController : BaseController
{
    public TcpServer tcpServer;
    protected override void Awake()
    {
        base.Awake();
        tcpServer = GameManager.GetInstance.tcpServer;
        tcpServer.AddEventListener(ProtoType.T_S_GameStart, GameStartCmd);
        tcpServer.AddEventListener(ProtoType.T_S_ClientInfo, SyncClientInfo);

    }

    private void SyncClientInfo(string obj)
    {
        Debug.Log("***********接收客户端玩家信息**************");
        Debug.Log(obj);
    }

    private void GameStartCmd(string sceneName)
    {
        Debug.Log("有玩家连接，游戏开始：" + sceneName);
        ScenesManager.GetInstance.LoadNextScene(sceneName, () =>
        {
            GameObject.Find("BG").SetActive(false);
            MessageDispatcher.Dispatch(MessagesType.VRChangeScene, sceneName);
            GameManager.GetInstance.SendMessage(ProtoType.T_C_HostInfo, GameManager.GetInstance.userName);
        });
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
