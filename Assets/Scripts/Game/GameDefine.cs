#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             GameDefine
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeviceType
{
    Pad =1,
    Pico =2
}
public enum EUiId
{
    NullUI,
    MainView,
    LoginView,
    ActiviteView,
    ErrorBoxView,//错误弹窗
    SceneSelectView,
    PadControlViewOne,
    PadControlViewTwo,
    MessageBoxView,
    ReleaseResView,
    VersionView,//版本提示ui
}
public class GameDefine  {
    public const string key = "100f23d0685dcde7dcb3814beb233340";//签名秘钥
    public const int GameFrameRate = 30;                       //游戏帧频
    public const string AppName = "吐司先生";           //应用程序名称
    public const string ExtName = ".assetbundle";              //素材扩展名
    public const string AssetDirname = "StreamingAssets";      //素材目录 


    public class DefinePath {
        public static string xmlDataPath = "Cfg/";
        public static string musicPath = "Audio/Music/";
        public static string soundPath = "Audio/Sound/";
    }
    public static Dictionary<EUiId, string> dicUIPath = new Dictionary<EUiId, string>()
      {
        {EUiId.MainView,"Prefabs/UI/"+"MainView" },
        {EUiId.LoginView,"Prefabs/UI/"+"LoginView" },
        {EUiId.ErrorBoxView,"Prefabs/UI/"+"ErrorBoxView" },
        {EUiId.ActiviteView,"Prefabs/UI/"+"ActiviteView" },
        {EUiId.SceneSelectView,"Prefabs/UI/"+"SceneSelectView" },
        {EUiId.PadControlViewOne,"Prefabs/UI/"+"PadControlViewOne" },
        {EUiId.PadControlViewTwo,"Prefabs/UI/"+"PadControlViewTwo" },
        {EUiId.ReleaseResView,"Prefabs/UI/"+"ReleaseResView" },
    };

    public class SceneName
    {
        public static string MainScene = "MainScene";
    }
    public class AudioPath
    {
        public static string MUSIC = "Art/Music/";
    }
    public class PrefabPath
    {
        public static string PREFAB_EFFECT = "Prefabs/Effects/";
        public static string PREFAB_UI = "Prefabs/UI/";
        public static string PREFAB_ENTITY = "Prefabs/Entity/";
        public static string ANIMAL = "Prefabs/Animal/";


    }
    public class SpriteBundleName
    {
        public static string AgreementImage = "agreement/image";
    }

    //本地存储字段
    public class LocalStorage
    {
        public const string VersionNum = "version";//存储应用当前的版本号
        public const string BundDevicesID = "BundDevicesID";//绑定设备的设备id
        public const string BindID = "BindID";//绑定ID
        public const string IsBinding = "IsBinding";//是否绑定
        public const string IsActivate = "IsActivate";//是否激活
        public const string IsRegister = "IsRegister";//是否注册sn
        public const string SN = "SN";//激活码

        public const string Version = "Version";//当前版本
        public const string OtherVersion = "OtherVersion";//配对设备的版本
        public const string ReleaseComplete = "ReleaseComplete";//资源已经释放

    }
    #region
    /// <summary>
    /// 取得数据存放目录
    /// </summary>
    public static string DataPath
    {
        get
        {
            string game = AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/" + game + "/";
            }
            return "c:/" + game + "/";
        }
    }
    public static string GetRelativePath()
    {
        if (Application.isEditor)
            return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + GameDefine.AssetDirname + "/";
        else if (Application.isMobilePlatform || Application.isConsolePlatform)
            return "file:///" + DataPath;
        else // For standalone player.
            return "file://" + Application.streamingAssetsPath + "/";
    }
    /// <summary>
    /// 应用程序内容路径
    /// </summary>
    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.streamingAssetsPath;
                //path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/" + GameDefine.AssetDirname + "/";
                break;
        }
        return path;
    }
    #endregion
}
