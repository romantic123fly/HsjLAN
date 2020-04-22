using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiviteView : BaseView {

    private InputField codeInput;
    private Button activateBtn;
    Transform tipGo;

    private Text tips;
    protected override void InitUIData()
    {
        base.InitUIData();
        uiId = EUiId.ActiviteView;
    }
    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();
        codeInput = GameTool.FindTheChild(gameObject, "CodeInput").GetComponent<InputField>();
        activateBtn = GameTool.FindTheChild(gameObject, "Activite").GetComponent<Button>();
        tipGo = GameTool.FindTheChild(gameObject, "TipsBg").GetComponent<Transform>();

        tips = GameTool.FindTheChild(gameObject, "Tips").GetComponent<Text>();
        tipGo.gameObject.SetActive(false);
        activateBtn.onClick.AddListener(SendActivitePost);
    } 
    private void SendActivitePost()
    {
        if (codeInput.text ==""|| codeInput.text==null)
        {
            tips.text = "请输入有效的激活码";
            return;
        }
    }
    public void SetTips(string str)
    {
        tipGo.gameObject.SetActive(true);
        tips.text = str;
    }
}
