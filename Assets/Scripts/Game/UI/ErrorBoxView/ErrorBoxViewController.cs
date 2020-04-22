using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorBoxViewController : BaseController {

    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);
        switch(messages.Type)
        {
            case MessagesType.ShowErrorView:
                var data = messages.Data.ToString();
                UIManager.GetInstance.ShowUI(EUiId.ErrorBoxView,true, data); break;
            case MessagesType.HideErrorView:
                UIManager.GetInstance.HideUI(EUiId.ErrorBoxView); break;
            default:
                break;
        }
    }
}
