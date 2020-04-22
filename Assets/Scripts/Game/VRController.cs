#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             PlayerController
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class VRController : BaseController
{
    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);

        switch (messages.Type)
        {
            case MessagesType.VRChangeScene:

                Debug.LogError("SceneNmae" );
                break;
            case MessagesType.EnterPark:
              
                break;
            case MessagesType.VRBeganVisiting:
             
                break;
            case MessagesType.VRStopVisiting:
              
                break;
            case MessagesType.VRReset:
                break;
            case MessagesType.VRStartShow:
               
                break;
            ///开始宣讲
            case MessagesType.VrPreach:
             
                break;
            case MessagesType.SetCurrentStage00:
           
                break;
            default:
                break;
        }
    }
}
