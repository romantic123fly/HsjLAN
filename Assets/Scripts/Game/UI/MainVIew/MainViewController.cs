#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             MainViewController
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainViewController : BaseController {

    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);
        switch (messages.Type)
        {
            case MessagesType.ShowMainView:
                UIManager.GetInstance.ShowUI(EUiId.MainView,false);
                break;
            case MessagesType.HideMainView:
                UIManager.GetInstance.HideUI(EUiId.MainView);
                break;
            default:
                break;
        }
    }
}
