using System.Net.Mime;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Events;

public class MyEventArgs
{
    public string btnName;
    public GameObject obj;
}
public class PadControlViewOne : BaseView
{

    protected override void InitUIData()
    {
        base.InitUIData();
    }

    //Pad控制界面返回选择场景
    public void TcpClientConnect()
    {
        ScenesManager.GetInstance.LoadNextScene("TempScene", () =>
        {
            GameObject.Find("Pvr_UnitySDK").gameObject.SetActive(false);
            GameManager.GetInstance.tcpClient.Connect(GameManager.GetInstance.CurHostIP, 7772);
            MessageDispatcher.Dispatch(MessagesType.ShowSceneSelectView);
        });
    }
   
}
