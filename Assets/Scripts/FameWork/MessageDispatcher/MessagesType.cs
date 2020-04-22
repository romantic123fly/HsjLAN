#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             MessageType
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesType
{

    //Pad端事件
    public const string ShowMainView = "Show_Main_View";
    public const string HideMainView = "Hide_Main_View";

    public const string ShowLoginView = "Show_Login_View";
    public const string HideLoginView = "Hide_Login_View";

    public const string ShowActiviteView = "Show_Activite_View";
    public const string HideActiviteView = "Hide_Activite_View";

    public const string ShowErrorView = "Show_ErrorBox_View";
    public const string HideErrorView = "Hide_ErrorBox_View";

    public const string ShowSceneSelectView = "Show_SceneSelect_View";
    public const string HideScenSelect = "Hide_SceneSelect_View";

    public const string ShowPadControlViewOne = "Show_PadControl_ViewOne";
    public const string HidePadControlViewOne = "Hide_PadControl_ViewOne";

   public const string ShowSuspendMask = "Show_Suspend_Mask";
    public const string HideSuspendMask = "Hide_Suspend_Mask";


    public const string PadSVisiting = "Pad_Visiting";
    public const string CreateDeviceBar = "Create_Device_Bar";
    public const string PadStopVisiting = "Pad_Stop_Visiting";
    public const string PadReset = "PadReset";
    
    public const string PadPreach = "PadPreach";
    public const string StartShow = "Start_Show";
    public const string StopPadHeartbeat = "StopPadHeartbeat";

    //Vr端事件
    public const string VRChangeScene = "VR_Change_Scene";
    public const string EnterPark = "Enter_Park";
    public const string VRBeganVisiting = "VR_Began_Visiting";
    public const string VRStopVisiting = "VR_Stop_Visiting";
    public const string VRStartShow = "VR_Start_Show";

    public const string VRReset = "VR_Reset";

    public const string SetStagePosZ = "SetStagePosY";
    public const string SetEulerX = "SetEulerX";
    public const string SetCurrentStage00 = "Stage00";
    
    public const string ShowOpenMouthTips = "ShowOpenMouthTips";
    public const string HideOpenMouthTips = "HideOpenMouthTips";
    public const string ShowDownLoadTips = "ShowDownLoadTips";
    public const string VrPreach = "VrPreach";
    public const string UPDATE_DOWNLOAD = "Update_Download";
    public const string UPDATE_PROGRESS = "Update_Progress";
    public const string UPDATE_EXTRACT = "Update_Extract";
    public const string StopVRHeartbeat = "StopVRHeartbeat";
}
