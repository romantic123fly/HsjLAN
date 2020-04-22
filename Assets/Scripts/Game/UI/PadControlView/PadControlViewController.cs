using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadControlViewController : BaseController
{
    public PadControlViewOne padControlViewOne;
    protected override void InitEvent()
    {
        base.InitEvent();
    }
    //public Transform stage;
    public override void HandleMessage(IMessages messages)
    {
        base.HandleMessage(messages);
        switch (messages.Type)
        {
            case MessagesType.ShowPadControlViewOne:
                padControlViewOne = UIManager.GetInstance.ShowUI(EUiId.PadControlViewOne) as PadControlViewOne;
                break;
            case MessagesType.HidePadControlViewOne:
                UIManager.GetInstance.HideUI(EUiId.PadControlViewOne);
                break;
            case MessagesType.ShowSuspendMask:
                break;
            case MessagesType.HideSuspendMask:
                break;
            case MessagesType.PadSVisiting:
                break;
            case MessagesType.PadStopVisiting:
                break;
            case MessagesType.PadReset:
                break;
            case MessagesType.StartShow:
                break;
            ///开始宣讲
            case MessagesType.PadPreach:
                break;
            default:
                break;
        }
    }
}
