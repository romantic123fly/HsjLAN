#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             MainView
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine.EventSystems;

public class MainView : BaseView
{
    private Button searchBtn;

    private List<DeviceListBar> barlist;
    Transform tipGo;
    private Text tips;
    protected override void InitUIOnAwake()
    {
        base.InitUIOnAwake();
        uiId = EUiId.MainView;
        _isSingleUse = true;
    }
    protected override void InitUIData()
    {
        base.InitUIData();
        barlist = new List<DeviceListBar>();
        searchBtn = GameTool.FindTheChild(gameObject, "Search").GetComponent<Button>();
        tipGo = GameTool.FindTheChild(gameObject, "TipsBg").GetComponent<Transform>();
        tips = GameTool.FindTheChild(gameObject, "TipsBg/Tips").GetComponent<Text>();
        tipGo.gameObject.SetActive(false);

        searchBtn.onClick.AddListener(Refresh);
    }
    protected override void Update()
    {
        base.Update();

        if (GameManager.GetInstance.deviceList.Count > 0)
        {
            foreach (var item in GameManager.GetInstance.deviceList)
            {
                if (barlist.Count > 0)
                {
                    if (barlist.FirstOrDefault(t => t.ip == item.Key) == null)
                    {
                        var a = ResourceManager.GetInstance.LoadPrefab("Prefabs/UI/DeviceBtn", GameObject.Find("DevicesList").transform);
                        var b = a.AddComponent<DeviceListBar>();
                        b.SetInfo(item.Key);
                        barlist.Add(b);
                    }
                }
                else
                {
                    var a = ResourceManager.GetInstance.LoadPrefab("Prefabs/UI/DeviceBtn", GameObject.Find("DevicesList").transform);
                    var b = a.AddComponent<DeviceListBar>();
                    b.SetInfo(item.Key);
                    barlist.Add(b);
                }
            }
            SetTips("");
        }
        else
        {
            SetTips("搜索不到设备");
        }
    }
    void Refresh()
    {
        if (barlist.Count > 0)
        {
            foreach (var item in barlist)
            {
                Destroy(item.gameObject);
            }
            barlist.Clear();
        }
        if (GameManager.GetInstance.deviceList.Count > 0)
        {
            foreach (var item in GameManager.GetInstance.deviceList)
            {
                var a = ResourceManager.GetInstance.LoadPrefab("Prefabs/UI/DeviceBtn", GameObject.Find("DevicesList").transform);
                var b = a.AddComponent<DeviceListBar>();
                b.SetInfo(item.Key);
                barlist.Add(b);
            }
            GameManager.GetInstance.deviceList.Clear();
            SetTips("");
        }
        else
        {
            SetTips("搜索不到设备");
        }
    }

    void InitList()
    {
        if (barlist.Count > 0)
        {
            foreach (var item in barlist)
            {
                Destroy(item.gameObject);
            }
            barlist.Clear();
        }

    }
    public void SetTips(string str)
    {
        tipGo.gameObject.SetActive(true);
        tips.text = str;
    }
}
