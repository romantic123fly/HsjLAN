#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             DeviceListBar
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceListBar : MonoBehaviour
{
    public Button deviceBar;
    public Text deviceIP;
    public string ip;
    // Use this for initialization
    void Awake()
    {
        deviceBar = gameObject.GetComponent<Button>();
        if (deviceBar != null)
        {
            deviceBar.onClick.AddListener(LinkHost);
        }
        deviceIP = gameObject.GetComponentInChildren<Text>();
    }
    public void SetInfo(string IP)
    {
        GameManager.GetInstance.CurHostIP = IP;
        ip = IP;
        if (deviceIP != null)
        {
            deviceIP.text = "房间 " + IP.Remove(0, IP.Length - 1) + "号";
            gameObject.name = "房间 " + IP.Remove(0, IP.Length - 1) + "号";
        }
    }
    //链接选中的host
    public void LinkHost()
    {
        GameManager.GetInstance.tcpClient.Connect(GameManager.GetInstance.CurHostIP, 7772);
        UdpManager.GetInstance.CloseUdp();
      
        ScenesManager.GetInstance.LoadNextScene("Scene01",()=> {
            GameManager.GetInstance.SendMessage(ProtoType.T_S_GameStart, "Scene01");
            UIManager.GetInstance.HideUI(EUiId.MainView);
        });
    }
}
