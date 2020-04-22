#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             BaseView
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelAfterHideUI();
public delegate void DelAfterShowUI();

public class BaseView : GameMonoBehaviour {
    //是否需要重置窗体反向切换的信息
    public bool isResetReturnUIInfor = false;
    public EUIRootType rootType = EUIRootType.Normal;
    public EShowUIMode showMode = EShowUIMode.NoReturn;
    public EUiId uiId = EUiId.NullUI;
    public EUiId beforeUiId = EUiId.NullUI;
    public bool _isSingleUse = false;
    public int currentDepth = 1;
    public int CurrentDepth
    {
        get
        {
            return currentDepth;
        }

        set
        {
            currentDepth = value;
        }
    }
    //是否需要添加到反向切换的信息里面，如果能就需要更新stackReturnInfor
    public bool isNeedUpdateStack
    {
        get
        {
            if (this.rootType == EUIRootType.KeepAbove)
            {
                return false;
            }
            if (this.showMode == EShowUIMode.NoReturn)
            {
                return false;
            }
            return true;
        }
    }
    protected Transform thisTrsns;

    protected override  void Awake()
    {
        base.Awake();
        thisTrsns = this.transform;
        InitUIData();
        InitUIOnAwake();
    }
    //初始化数据
    protected virtual void InitUIData()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x, gameObject.GetComponent<RectTransform>().sizeDelta.y) * (Screen.width / Screen.height);
    }
    //初始化界面元素
    protected virtual void InitUIOnAwake()
    {

    }
    //刷新ui数据
    public  override void Render()
    {
        base.Render();
    }
    public virtual void ShowUI()
    {
        gameObject.SetActive(true);
        Render();
    }
    public virtual void HideUI(DelAfterHideUI del = null)
    {
        if (_isSingleUse)
        {
            UIManager.GetInstance.dicAllUI.Remove(uiId);
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (del != null) del();

    }

    public virtual void Destroy()
    {
        BeforeDestroy();
        Destroy(gameObject);
    }
    protected virtual void BeforeDestroy()
    {

    }
    protected virtual void PlayAudio()
    {

    }
}
