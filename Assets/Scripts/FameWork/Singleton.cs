#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             Singleton
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class Singleton<T> : GameMonoBehaviour where T:Singleton<T> {
    public static  GameObject carrier;
    public static T instance;

    public static T GetInstance
    {
        get
        {
            if (instance == null)
            {
                if (carrier == null)
                {
                    carrier = new GameObject();
                }
                instance = carrier.AddComponent(typeof(T)) as T;
                instance.name = typeof(T).Name;

            }
            return instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    protected override void Start () {
     
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        //UnityEngine.Debug.Log(name + " __Destroy");
    }
   
}
