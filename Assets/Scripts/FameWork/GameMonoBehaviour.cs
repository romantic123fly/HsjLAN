#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             GameMonoBehaviour
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMonoBehaviour : MonoBehaviour {

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Awake () {
        InitView();
        Render();
    }
    /// <summary>
    /// 第一次Update前调用
    /// </summary>
    protected virtual void Start()
    {

    }
    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }
    /// <summary>
    /// 生命周期结束
    /// </summary>
    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 初始化事件
    /// </summary>
    protected virtual void InitEvent()
    {

    }

    /// <summary>
    /// 初始化界面元素
    /// </summary>
    protected virtual void InitView()
    {

    }
    /// <summary>
    /// 界面元素赋值
    /// </summary>
    public virtual void Render()
    {
        
    }
 
    // Update is called once per frame
    protected virtual void  Update () {
		
	}
}
