#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             UIManager
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
public enum EUIRootType
{
    KeepAbove,//中间层
    Normal,//底层
    TopUIRoot//最上层
}

//窗体的打开模式
public enum EShowUIMode
{
    DoNoting,
    HideOther,//需要反向切换（隐藏其他窗体）
    NoReturn//不需要反向切换(隐藏所有窗体包括最前方的窗体)
}

public class UIReturnInfor
{
    //将要被显示的窗体
    public BaseView willBeShowUI;
    //存放反向切换窗体id的列表
    public List<EUiId> listReturn;
}
public class UIManager : BaseManager<UIManager>
{
    //缓存所有窗体
    public Dictionary<EUiId, BaseView> dicAllUI;
    //缓存已经打开的窗体（正在显示的窗体）
    public Dictionary<EUiId, BaseView> dicShownUI;
    //缓存具有反向切换信息的UI栈体
    private Stack<UIReturnInfor> stackReturnInfor;
    //缓存当前窗体的ID
    public BaseView currentUI = null;
    //跳转到该窗体的上一个窗体
    //private BaseView beforeUI = null;

    //缓存UGUI的UIRoot
    public Transform uiRootTrans;
    //保持在最前方窗体的父节点
    private Transform KeepAboveUIRoot;
    //普通的弹出窗体的父节点     
    private Transform normalUIRoot;

    private Transform topUIRoot;

    public List<Vector3> pos = new List<Vector3>()
    {
        new Vector3(0,0,0),
         new Vector3(-710,0,0),
          new Vector3(710,0,0),
           new Vector3(0,1100,0),
            new Vector3(0,-1100,0),
    };

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (uiRootTrans == null)
        {
            uiRootTrans = GameObject.Find("Canvas").transform;
        }
        if (dicAllUI == null)
        {
            dicAllUI = new Dictionary<EUiId, BaseView>();
        }
        if (dicShownUI == null)
        {
            dicShownUI = new Dictionary<EUiId, BaseView>();
        }
        if (stackReturnInfor == null)
        {
            stackReturnInfor = new Stack<UIReturnInfor>();
        }
        //instance = this;
        InitUIManager();
    }
    public void InitUIManager()
    {
        //清空缓存的窗体
        if (dicAllUI != null)
        {
            dicAllUI.Clear();
        }
        if (dicShownUI != null)
        {
            dicShownUI.Clear();
        }
        DontDestroyOnLoad(uiRootTrans);
        //初始化显示窗体的父节点
        if (normalUIRoot == null)
        {
            normalUIRoot = new GameObject("NormalUIRoot").transform;
            GameTool.AddChildToParent(uiRootTrans, normalUIRoot);
            GameTool.SetLayer(uiRootTrans.gameObject.layer, normalUIRoot);
        }
        if (KeepAboveUIRoot == null)
        {
            KeepAboveUIRoot = new GameObject("KeepAboveUIRoot").transform;
            GameTool.AddChildToParent(uiRootTrans, KeepAboveUIRoot);
            GameTool.SetLayer(uiRootTrans.gameObject.layer, KeepAboveUIRoot);
        }
        if (topUIRoot == null)
        {
            topUIRoot = new GameObject("TopUIRoot").transform;
            GameTool.AddChildToParent(uiRootTrans, topUIRoot);
            GameTool.SetLayer(uiRootTrans.gameObject.layer, topUIRoot);
        }
    }
    public BaseView ShowUI(EUiId uiId, bool isABLoad =true, string tips = "")
    {
        BaseView baseUI = JudgeShowUI(uiId);
        if (baseUI != null)
        {
            baseUI.ShowUI();

            dicShownUI[uiId] = baseUI;
            if (baseUI.isResetReturnUIInfor)
            {
                //重置反向切换的信息（当显示的是Mainui的时候就需要重置）
                ClearStackReturnInfor();
            }
            if (uiId == EUiId.ErrorBoxView)
            {
                (baseUI as ErrorBoxView).SetContent(tips);
            }
            if (baseUI.rootType == EUIRootType.Normal)
            {
                //beforeUI = currentUI;
                currentUI = baseUI;
            }
            Debug.Log("显示："+uiId);
            return baseUI;
        }
        else
        {
            return null;
        }
    }

    //清空栈内元素
    public void ClearStackReturnInfor()
    {
        if (stackReturnInfor != null)
        {
            stackReturnInfor.Clear();
        }
    }
    public BaseView JudgeShowUI(EUiId uiID)
    {
        //先判断窗体是不是正在显示（已经显出）
        if (dicShownUI.ContainsKey(uiID))//已经显示出来了
        {
            GetViewByUiID(uiID).Render();
            return null;
        }
        BaseView baseUI = GetBaseUI(uiID);

        //如果baseui在字典里面没有查找到，说明该窗体还没有加载过
        if (baseUI == null)
        {
            //开始加载
            if (GameDefine.dicUIPath.ContainsKey(uiID))
            {
                GameObject theUI = null;
         
                    string path = GameDefine.dicUIPath[uiID];
                    theUI = ResourceManager.GetInstance.LoadPrefab(path, null);
                
                if (theUI != null)
                {
                    theUI.SetActive(true);
                    //把窗体放到对应的节点下面
                    baseUI = theUI.GetComponent<BaseView>();
                    Transform theRoot = GetUIRoot(baseUI);
                    GameTool.AddChildToParent(theRoot, theUI.transform);
                    theUI = null;
                    dicAllUI[uiID] = baseUI;
                }
                else
                {
                    Debug.LogError("Load UI Null");
                }
            }
        }
        UpDateStack(baseUI);
        return baseUI;
    }
    //把窗体添加进栈stackReturnInfor
    public void UpDateStack(BaseView baseUI)
    {
        if (baseUI.isNeedUpdateStack)
        {
            //将要移除的窗体的id列表
            List<EUiId> removeKey = null;

            //存放需要添加进栈的窗体列表
            List<BaseView> maxToMinUi = new List<BaseView>();

            //存放depth值从大到小排序后的窗体id
            List<EUiId> newList = new List<EUiId>();

            UIReturnInfor uiReturnInfor = new UIReturnInfor();

            if (dicShownUI.Count > 0)
            {
                foreach (KeyValuePair<EUiId, BaseView> item in dicShownUI)
                {
                    if (item.Value.rootType != EUIRootType.KeepAbove)
                    {
                        item.Value.HideUI();
                        if (removeKey == null)
                        {
                            removeKey = new List<EUiId>();
                        }
                        removeKey.Add(item.Key);
                    }
                    if (item.Value.isNeedUpdateStack)
                    {
                        maxToMinUi.Add(item.Value);
                    }
                }
                if (removeKey != null)
                {
                    for (int i = 0; i < removeKey.Count; i++)
                    {
                        dicShownUI.Remove(removeKey[i]);
                    }
                }
                //把进栈的窗体按照depth值从大到小排序
                if (maxToMinUi.Count > 0)
                {
                    maxToMinUi.Sort(delegate (BaseView a, BaseView b) { return a.CurrentDepth.CompareTo(b.CurrentDepth); });

                    for (int i = 0; i < maxToMinUi.Count; i++)
                    {
                        newList.Add(maxToMinUi[i].uiId);
                    }

                    uiReturnInfor.willBeShowUI = baseUI;
                    uiReturnInfor.listReturn = newList;
                    stackReturnInfor.Push(uiReturnInfor);
                }
            }
        }
        else
        {
            if (baseUI.showMode == EShowUIMode.NoReturn)
            {
                HideAllUI(true);
            }
        }
        CheckStack(baseUI);
    }
    //检测栈的顺序是否被打乱。如果被打乱了就清空栈
    public void CheckStack(BaseView baseUi)
    {
        if (baseUi.isNeedUpdateStack)
        {
            if (stackReturnInfor.Count > 0)
            {
                //peek是取出栈顶的栈顶元素
                UIReturnInfor uiReturnInfor = stackReturnInfor.Peek();
                if (uiReturnInfor.willBeShowUI != baseUi)//说明栈被打乱
                {
                    stackReturnInfor.Clear();
                }
            }
        }
    }
    //点击返回按钮
    public void ClickReturn()
    {
        if (stackReturnInfor.Count == 0)//说明没有反向切换的信息
        {
            if (currentUI == null) return;
            EUiId beforeUiId = currentUI.beforeUiId;
            if (beforeUiId != EUiId.NullUI)
            {
                HideUI(currentUI.uiId, delegate { ShowUI(beforeUiId); });
            }
        }
        else //说明有反向切换信息
        {
            UIReturnInfor uiReturnInfor = stackReturnInfor.Peek();
            if (uiReturnInfor != null)
            {
                //获得当前窗体的ui
                EUiId theId = uiReturnInfor.willBeShowUI.uiId;
                if (dicShownUI.ContainsKey(theId))
                {
                    HideUI(theId, delegate
                    {
                        //如果是第一个窗体（depth值最大的窗体），并且没有显示出来
                        if (!dicShownUI.ContainsKey(uiReturnInfor.listReturn[0]))
                        {
                            BaseView baseUI = GetBaseUI(uiReturnInfor.listReturn[0]);
                            baseUI.ShowUI();
                            dicShownUI[baseUI.uiId] = baseUI;

                            //this.beforeUI = currentUI;
                            currentUI = baseUI;
                            //pop是把栈顶元素删除
                            stackReturnInfor.Pop();
                        }
                    });
                }
            }
        }
    }
    //获取当前窗体的根节点
    public Transform GetUIRoot(BaseView baseUI)
    {
        if (baseUI.rootType == EUIRootType.Normal)
        {
            return normalUIRoot;
        }
        if (baseUI.rootType == EUIRootType.KeepAbove)
        {
            return KeepAboveUIRoot;
        }
        if (baseUI.rootType == EUIRootType.TopUIRoot)
        {
            return topUIRoot;
        }
        else
        {
            return uiRootTrans;
        }
    }
    //判断当前ui是否被加载过
    public BaseView GetBaseUI(EUiId uiId)
    {
        if (dicAllUI.ContainsKey(uiId))
        {
            return dicAllUI[uiId];
        }
        else
        {
            return null;
        }
    }

    //隐藏该窗体
    public void HideUI(EUiId uiId, DelAfterHideUI del = null)
    {
        if (del != null) del();
        //如果该窗体没有显示出来，就不用隐藏了
        if (dicShownUI.ContainsKey(uiId))
        {
            dicShownUI[uiId].HideUI();
            dicShownUI.Remove(uiId);
        }
    }

    //隐藏所有窗体
    public void HideAllUI(bool isHideAboveUI)
    {
        List<EUiId> listRemove = null;
        if (isHideAboveUI)//需要隐藏最前端ui
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in dicShownUI)
            {
                uiItem.Value.HideUI();
            }
            dicShownUI.Clear();
        }
        else
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in dicShownUI)
            {
                if (uiItem.Value.rootType == EUIRootType.KeepAbove) continue;
                else
                {
                    if (listRemove == null)
                    {
                        listRemove = new List<EUiId>();
                        listRemove.Add(uiItem.Key);
                        uiItem.Value.HideUI();
                    }
                }
            }
        }
        //把隐藏的窗体从dicShowUI中移除
        if (listRemove != null)
        {
            for (int i = 0; i < listRemove.Count; i++)
            {
                dicShownUI.Remove(listRemove[i]);
            }
        }

        if (topUIRoot.childCount > 0)
        {
            Destroy(topUIRoot.GetChild(0).gameObject);
        }
    }
  
    //获取指定已显示的窗体
    public BaseView GetViewByUiID(EUiId eUiId)
    {
        BaseView tempView = null;
        foreach (var item in dicShownUI)
        {
            if (item.Key == eUiId)
            {
                tempView = dicShownUI[eUiId];
                break;
            }
        }
        return tempView;
    }

    public void DestroyUI(EUiId uiId, DelAfterHideUI del)
    {
        //如果该窗体没有显示出来，就不用隐藏了
        if (dicAllUI.ContainsKey(uiId))
        {
            dicAllUI[uiId].Destroy();
            dicAllUI.Remove(uiId);
            if (dicShownUI.ContainsKey(uiId))
            {
                dicShownUI.Remove(uiId);
            }
        }
    }
    public void DestoryAllUI()
    {
        foreach (var item in dicAllUI)
        {
            item.Value.Destroy();
        }

        dicShownUI.Clear();
        dicAllUI.Clear();
    }


    public void AssignViewRender(EUiId eUiId)
    {
        //先判断窗体是不是正在显示（已经显出）
        if (dicShownUI.ContainsKey(eUiId))//已经显示出来了
        {
            dicShownUI[eUiId].Render();
        }
    }
}




