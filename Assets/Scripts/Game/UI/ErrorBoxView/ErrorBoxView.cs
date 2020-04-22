using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorBoxView : BaseView {

    public Text content;
    public Button confirmBtn;
    protected override void InitUIData()
    {
        base.InitUIData();
        uiId = EUiId.ErrorBoxView;
        rootType = EUIRootType.KeepAbove;
    }
    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();
        content = GameTool.FindTheChild(gameObject, "Bg/Content").GetComponent<Text>();
        confirmBtn = GameTool.FindTheChild(gameObject, "Bg/Confirm").GetComponent<Button>();
        confirmBtn.onClick.AddListener(() => { MessageDispatcher.Dispatch(MessagesType.HideErrorView); });
    }
    public  void SetContent(string conten)
    {
        content.text = conten;
    }
}
