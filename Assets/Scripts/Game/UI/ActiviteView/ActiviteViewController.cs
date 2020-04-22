
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiviteViewController : BaseController {

    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);
        switch (messages.Type)
        {
            case MessagesType.ShowActiviteView:
                UIManager.GetInstance.ShowUI(EUiId.ActiviteView);
                break;
            case MessagesType.HideActiviteView:
                UIManager.GetInstance.HideUI(EUiId.ActiviteView);
                break;
            default:
                break;
        }
    }
} 
