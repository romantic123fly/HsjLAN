#region 模块信息
// **********************************************************************
// Copyright (C) 2019 jiamiantech
// Please contact me if you have any questions
// File Name:             WriteLog
// Author:                幻世界
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class WriteLog : MonoBehaviour
{
    private static FileStream FileWriter;
    private static UTF8Encoding encoding;
    private static WriteLog consoleLog;

    public static WriteLog ConsoleLog //开启单例
    {
        get
        {
            if (consoleLog == null)
                consoleLog = new WriteLog();
            return consoleLog;
        }
    }
   
    public void LogStart()
    {
        Debug.LogError("开始写入日志");
        Debug.Log(Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + "/Log"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Log");
        }

        string NowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "_").Replace("/", "_").Replace(":", "_");
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/Log/" + NowTime + "_Log.txt");
        //设置Log文件输出地址
        FileWriter = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        encoding = new UTF8Encoding();
        Application.logMessageReceived += LogCallback;
    }
    /// <summary>
    /// Log回调
    /// </summary>
    /// <param name="condition">输出内容</param>
    /// <param name="stackTrace">堆栈追踪</param>
    /// <param name="type">Log日志类型</param>
    private void LogCallback(string condition, string stackTrace, LogType type) //写入控制台数据
    {
        //输出的日志类型可以自定义
        string content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + "【" + type + "】" + "【" + stackTrace + "】" + ":" + condition + Environment.NewLine+ Environment.NewLine;
        FileWriter.Write(encoding.GetBytes(content), 0, encoding.GetByteCount(content));
        FileWriter.Flush();
    }

    private void OnDestroy() //关闭写入
    {
        if ((FileWriter != null))
        {
            Debug.LogError("日志写入结束");
            FileWriter.Close();
            Application.logMessageReceived-= LogCallback;
        }
    }
}

