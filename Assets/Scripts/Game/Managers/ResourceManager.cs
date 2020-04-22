#region 模块信息
using System.Text;
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             ResourceManager
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class ResourceManager : BaseManager<ResourceManager>
{
    public GameObject LoadPrefab(string abName, Transform parent = null)
    {
        GameObject prefab = Load(abName) as GameObject;
        if (prefab == null)
        {
            Debug.LogWarning("Can't find prefab:" + abName);
            return null;
        }
        var go = UnityEngine.Object.Instantiate(prefab) as GameObject;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localPosition = Vector3.zero;
        go.transform.SetParent(parent);
        return go;
    }

    public static UnityEngine.Object Load(string path)
    {
        var res = Resources.LoadAsync(path);
        if (res == null)
        {
            return new UnityEngine.Object();
        }
        return res.asset;
    }

    public  AudioClip LoadAudioClip(string v1, string v2)
    {
        return null;
    }
}
