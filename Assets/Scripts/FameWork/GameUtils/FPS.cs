#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             FPS
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Profiling;

//显示FPS的组件，用OnGUI新式,性能检测器，输出各种信息
public class FPS : MonoBehaviour
{
    float _nextTime;
    int frameCount;
    int fps = 0;
    //MemInfo memInfo;
    void Start()
    {
        //Application.targetFrameRate = GameDefine.GameFrameRate;

        _nextTime = Time.realtimeSinceStartup + 1;
        frameCount = 0;
    }
    void Update()
    {
        //memInfo.GetMemoryInfo();
        frameCount++;
        if (Time.realtimeSinceStartup > _nextTime)
        {
            fps = frameCount;
            frameCount = 0;
            _nextTime = Time.realtimeSinceStartup + 1;
        }
    }
    void OnGUI()
    {
        GUI.color = Color.green;
        string s = "fps:" + fps 
            + " mus:" + (Profiler.GetMonoUsedSizeLong() / 1024 / 1024).ToString("000")//为活动对象和非收集对象分配的托管内存。
            + "M" + " mhs:" + (Profiler.GetMonoHeapSizeLong() / 1024 / 1024).ToString("000")//返回用于管理内存的预留空间的大小。
            + "M" + " uhs:" + (Profiler.usedHeapSizeLong/ 1024 / 1024).ToString("000")//程序使用的堆大小。
            + "M";
        //+"---"+memInfo.VmRss+"---";
        GUI.Label(new Rect(Screen.width/2, 0, 1000, 500), s);
    }
}