#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             Main
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    private bool isClient;
    private bool isServer;

    public bool IsClient
    {
        get => isClient;
        set
        {
            isClient = value;
            if (isClient)
            {
                GameManager.GetInstance.isServer = false;

                GameManager.GetInstance.userName = GameObject.Find("UserName").GetComponent<InputField>().text == "" ? GameManager.GetInstance.userName : GameObject.Find("UserName").GetComponent<InputField>().text;

                MessageDispatcher.Dispatch(MessagesType.ShowMainView);
                UdpManager.GetInstance.ListenMes();
                GameObject.Find("BG").SetActive(false);
                if (GameManager.GetInstance.tcpClient == null)
                {
                    GameManager.GetInstance.tcpClient = GameObject.Find("Main").AddComponent<TcpClient>();
                    GameObject.Find("Controller").gameObject.AddComponent<ClientController>();
                }
            }
        }
    }
    public bool IsServer
    {
        get => isServer; set
        {
            isServer = value;

            if (IsServer)
            {
                GameManager.GetInstance.userName = GameObject.Find("UserName").GetComponent<InputField>().text == "" ? GameManager.GetInstance.userName : GameObject.Find("UserName").GetComponent<InputField>().text;
                GameManager.GetInstance.isServer = true;
                GameManager.GetInstance.StartUdpCheck();
                GameObject.Find("LinkTips").GetComponent<Text>().text = "房间" + GameTool.GetLocalIP().Remove(0, GameTool.GetLocalIP().Length - 1) + "号 创建成功等待连接!!!";
            }
        }
    }

    void Start()
    {
        gameObject.AddComponent<ShowConsole>();
        //WriteLog.ConsoleLog.LogStart();
        Application.runInBackground = true;
        InitManager();
        InitController();
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// 初始化Controller
    /// </summary>
    private void InitController()
    {
        GameObject controller = new GameObject("Controller");
        DontDestroyOnLoad(controller);
        controller.AddComponent<VRController>();
        controller.AddComponent<MainViewController>();
        controller.AddComponent<ErrorBoxViewController>();
        controller.AddComponent<ActiviteViewController>();
        controller.AddComponent<PadControlViewController>();
    }
    /// <summary>
    /// 初始化Manager
    /// </summary>
    private void InitManager()
    {
        Debug.Log("当前设备分辨率：" + Screen.width + " X " + Screen.height);
        GameManager.GetInstance.SetUp();
        ResourceManager.GetInstance.SetUp();
        UdpManager.GetInstance.SetUp();
        UIManager.GetInstance.SetUp();

        ScenesManager.GetInstance.SetUp();

        AudioManager.GetInstance.SetUp();
    }

}
