#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             GameTool
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameTool : MonoBehaviour
{
    public static string GetLocalIP()
    {
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());   //Dns.GetHostName()获取本机名Dns.GetHostAddresses()根据本机名获取ip地址组
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();  //ipv4
            }
        }
        return null;

    }
    //获取指定范围内的随机整数
    public static int GetRandomInt(int num1, int num2)
    {
        if (num1 < num2)
        {
            return UnityEngine.Random.Range(num1, num2);
        }
        else
        {
            return UnityEngine.Random.Range(num2, num1);
        }
    }
    //生成随机字符串
    public static string RandomCharacters()
    {
        string str = string.Empty;
        long num2 = DateTime.Now.Ticks + 0;
        System.Random random = new System.Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> 1)));
        for (int i = 0; i < 20; i++)
        {
            char ch;
            int num = random.Next();
            if ((num % 2) == 0)
            {
                ch = (char)(0x30 + ((ushort)(num % 10)));
            }
            else
            {
                ch = (char)(0x41 + ((ushort)(num % 0x1a)));
            }
            str = str + ch.ToString();
        }
        return str;
    }
    //byte[]转string后再转byte[]
    public static byte[] byteStringTobyte(string bstr)
    {
        string[] sa = bstr.Substring(1, bstr.Length - 2).Split(',');
        byte[] barr = new byte[sa.Length];
        try
        {
            for (int i = 0; i < barr.Length; i++)
            {
                barr[i] = byte.Parse(sa[i]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        return barr;
    }
    //读取本地图片转byte[]
    public static byte[] ReadTexture(string path)
    {
        Debug.Log(" @ ! the texture path is + !!    " + path);
        FileStream fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] buffer = new byte[fileStream.Length]; //创建文件长度的buffer   
        fileStream.Read(buffer, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return buffer;
    }
    //清理内存（一般在切换场景的时候调用）
    public static void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }
    //根据特别符号分割字符串
    public static string[] DivisionStr(string str, char a)
    {
        return str.Split(a);
    }

    //删除指定位置 长度的字符串(去掉克隆出来的物体后面的clone)
    public static string SubString(string str)
    {
        return str.Substring(0, str.Length - 7);
    }
    //字符串转枚举类型
    public static T StringToEnum<T>(string str)
    {

        return (T)Enum.Parse(typeof(T), str);
    }
    //世界坐标转为屏幕坐标
    public static Vector3 WorldPointToScenePoint(Transform position1)
    {
        return Camera.main.WorldToScreenPoint(position1.position);
    }

    //封装PlayerPrefs
    public static bool HasKey(string keyName)
    {
        return PlayerPrefs.HasKey(keyName);
    }
    public static int GetInt(string keyName)
    {
        return PlayerPrefs.GetInt(keyName);
    }
    public static void SetInt(string keyName, int valueInt)
    {
        PlayerPrefs.SetInt(keyName, valueInt);
    }
    public static float GetFloat(string keyName)
    {
        return PlayerPrefs.GetFloat(keyName);
    }
    public static void SetFloat(string keyName, float valueFloat)
    {
        PlayerPrefs.SetFloat(keyName, valueFloat);
    }
    public static string GetString(string keyName)
    {
        return PlayerPrefs.GetString(keyName);
    }
    public static void SetString(string keyName, string valueString)
    {
        PlayerPrefs.SetString(keyName, valueString);
    }
    public static void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void DeleteTheKey(string keyNmae)
    {
        PlayerPrefs.DeleteKey(keyNmae);
    }

    //查找物体的方法
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    //获取子物体的脚本
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    //给子物体添加脚本
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            T[] theComponentsArr = searchTrans.GetComponents<T>();
            for (int i = 0; i < theComponentsArr.Length; i++)
            {
                if (theComponentsArr[i] != null)
                {
                    Destroy(theComponentsArr[i]);
                }
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }
    //添加子物体
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        childTrs.SetParent(parentTrs);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
        SetLayer(parentTrs.gameObject.layer, childTrs);
    }
    //设置所有子物体的Layer
    public static void SetLayer(int parentLayer, Transform childTrs)
    {
        childTrs.gameObject.layer = parentLayer;
        for (int i = 0; i < childTrs.childCount; i++)
        {
            Transform child = childTrs.GetChild(i);
            child.gameObject.layer = parentLayer;
            SetLayer(parentLayer, child);
        }
    }

    /// <summary>
    /// 判断是否点击在UI上(此处忽略覆盖在上层的UI及背景UI)
    /// </summary>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        //print(raycastResults.Count);
        return raycastResults.Count > 0;
    }

    //写进去
    public static void WriteFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo fi = new FileInfo(path + "/" + name);
        sw = fi.CreateText();
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
    //读出来
    public static string ReadFile(string path, string name)
    {
        StreamReader sr;
        FileInfo fi = new FileInfo(path + "/" + name);
        sr = fi.OpenText();
        string info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();

        return info;
    }
    public static void DebugProtoData(byte[] buffer, int MdmNum, int CmdNum, bool isSend)
    {
        string path = "";
        if (isSend) path = Application.dataPath + "/Data/SendData";
        else path = Application.dataPath + "/Data/ReceiveData";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string str = "0";
        foreach (var item in buffer)
        {
            str += item + ",";
        }
        WriteFile(path, "M" + MdmNum + "C" + CmdNum, str);
    }
    //c#调用java代码实现在android中打印吐司
    public static void MakeToast(string info)
    {
#if UNITY_ANDROID
        AndroidJavaObject currentActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            Toast.CallStatic<AndroidJavaObject>("makeText", currentActivity, info, Toast.GetStatic<int>("LENGTH_LONG")).Call("show");
        }
        ));
#elif UNITY_IPHONE
        Debug.Log("【"+info+"】");
#endif
    }
    //秒转换为标准时间格式
    public static string Sec2vtm(int seconds, int mode = 0)
    {
        string str = "";
        //int hour = seconds % (3600 * 24) * 24 + seconds % (3600 * 24) / 3600;
        int hour = seconds / 3600;
        int minute = (seconds - hour * 3600) / 60;
        int sec = seconds % 60;

        string hourStr = (hour < 10) ? "0" + hour : hour.ToString();
        string minStr = (minute < 10) ? "0" + minute : minute.ToString();
        string secStr = (sec < 10) ? "0" + sec : sec.ToString();
        switch (mode)
        {
            case 1:
                {
                    str = hourStr;
                    break;
                }
            case 2:
                {
                    str = minStr;
                    break;
                }
            case 3:
                {
                    str = secStr;
                    break;
                }
            case 5:
                {
                    str = hourStr + ":" + minStr;
                    break;
                }
            case 6:
                {
                    str = minStr + ":" + secStr;
                    break;
                }
            default:
                {
                    str = hourStr + ":" + minStr + ":" + secStr;
                    break;
                }
        }
        return str;
    }
    public static string Sec2vtm2(int seconds, int mode = 0)
    {
        string str = "";
        //int hour = seconds % (3600 * 24) / 3600;
        //int minute = seconds % 3600 / 60;
        //int sec = seconds % 60;
        int hour = seconds / 3600;
        int minute = (seconds - hour * 3600) / 60;
        int sec = seconds % 60;

        string hourStr = (hour < 10) ? "0" + hour : hour.ToString();
        string minStr = (minute < 10) ? "0" + minute : minute.ToString();
        string secStr = (sec < 10) ? "0" + sec : sec.ToString();
        switch (mode)
        {
            case 1:
                {
                    str = hourStr + "时";
                    break;
                }
            case 2:
                {
                    str = minStr + "分";
                    break;
                }
            case 3:
                {
                    str = secStr + "秒";
                    break;
                }
            case 5:
                {
                    str = hourStr + "时" + minStr + "分";
                    break;
                }
            case 6:
                {
                    str = minStr + "分" + secStr + "秒";
                    break;
                }
            default:
                {
                    str = hourStr + "时" + minStr + "分" + secStr + "秒";
                    break;
                }
        }
        return str;
    }


    // MD5  32位加密
    public static string UserMd5(string str)
    {
        string cl = str;
        StringBuilder pwd = new StringBuilder();
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            pwd.Append(s[i].ToString("x2"));
            //pwd = pwd + s[i].ToString("X");
        }
        return pwd.ToString();
    }


    //获取本地app列表
    public static List<string> GetAllApk()
    {
        List<string> apks = new List<string>();
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject packageInfos = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
            AndroidJavaObject[] packages = packageInfos.Call<AndroidJavaObject[]>("toArray");
            for (int i = 0; i < packages.Length; i++)
            {
                AndroidJavaObject applicationInfo = packages[i].Get<AndroidJavaObject>("applicationInfo");
                if ((applicationInfo.Get<int>("flags") & applicationInfo.GetStatic<int>("FLAG_SYSTEM")) == 0)// 判断是不是系统应用
                {
                    string packageName = applicationInfo.Get<string>("packageName");
                    AndroidJavaObject applicationLabel = packageManager.Call<AndroidJavaObject>("getApplicationLabel", applicationInfo);
                    string packageLable = applicationLabel.Call<string>("toString");
                    apks.Add(packageLable + "|" + packageName);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
#endif
        return apks;
    }
    //深度复制
    public static object DeepClone(object obj)
    {
        if (obj != null)
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            bFormatter.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);
            return bFormatter.Deserialize(stream);
        }
        return null;
    }
    public static object CopyObject(object obj)
    {
        object targetDeepCopyObj;

        if (obj == null)
        {
            Debug.Log("copy obj is null");
        }
        var targetType = obj.GetType();
        //值类型  
        if (targetType.IsValueType == true)
        {
            targetDeepCopyObj = obj;
        }
        //引用类型   
        else
        {
            targetDeepCopyObj = Activator.CreateInstance(targetType);   //创建引用对象   
            MemberInfo[] memberCollection = obj.GetType().GetMembers();

            foreach (MemberInfo member in memberCollection)
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    object fieldValue = field.GetValue(obj);
                    if (fieldValue is ICloneable)
                    {
                        field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                    }
                    else
                    {
                        field.SetValue(targetDeepCopyObj, CopyObject(fieldValue));
                    }

                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo myProperty = (PropertyInfo)member;
                    MethodInfo info = myProperty.GetSetMethod(false);
                    if (info != null)
                    {
                        object propertyValue = myProperty.GetValue(obj, null);
                        if (propertyValue is ICloneable)
                        {
                            myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                        }
                        else
                        {
                            myProperty.SetValue(targetDeepCopyObj, CopyObject(propertyValue), null);
                        }
                    }
                }
            }
        }
        return targetDeepCopyObj;
    }
    public static void CopyObject(object source, object target)
    {
        var targetType = target.GetType();
        if (targetType == null)
        {
            targetType = source.GetType();
        }
        //值类型  
        if (targetType.IsValueType == true)
        {
            target = source;
        }
        //引用类型   
        else
        {
            if (source == null)
            {
                return;
            }
            MemberInfo[] memberCollection = source.GetType().GetMembers();

            foreach (MemberInfo member in memberCollection)
            {
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    object fieldValue = field.GetValue(source);
                    if (fieldValue is ICloneable)
                    {
                        field.SetValue(target, (fieldValue as ICloneable).Clone());
                    }
                    else
                    {
                        field.SetValue(target, CopyObject(fieldValue));
                    }

                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo myProperty = (PropertyInfo)member;
                    MethodInfo info = myProperty.GetSetMethod(false);
                    if (info != null)
                    {
                        object propertyValue = myProperty.GetValue(source, null);
                        if (propertyValue is ICloneable)
                        {
                            myProperty.SetValue(target, (propertyValue as ICloneable).Clone(), null);
                        }
                        else
                        {
                            myProperty.SetValue(target, CopyObject(propertyValue), null);
                        }
                    }

                }
            }
        }
    }

    public static int ChangUnicodeToUTF16(string paramContent, StringBuilder tempSB, StringBuilder tmps, int i)
    {
        for (int maxln = 0; maxln < 20; maxln++)
        {
            if (paramContent[i] != '-' && paramContent[i] != '[')
            {  //向前探测1位
                tmps.Append(paramContent[i]);
                i++;
            }
            else
            {
                break;
            }
        }


        tempSB.Append(EmojiCodeToUTF16String(tmps.ToString()));

        tmps.Remove(0, tmps.Length);
        return i;
    }
    public static Int32 EmojiToUTF16(Int32 V, bool LowHeight = true)
    {

        Int32 Vx = V - 0x10000;

        Int32 Vh = Vx >> 10;//取高10位部分
        Int32 Vl = Vx & 0x3ff; //取低10位部分
                               //Response.Write("Vh:"); Response.Write(Convert.ToString(Vh, 2)); Response.Write("<br/>"); //2进制显示
                               //Response.Write("Vl:"); Response.Write(Convert.ToString(Vl, 2)); Response.Write("<br/>"); //2进制显示

        Int32 wh = 0xD800; //結果的前16位元初始值,这个地方应该是根据Unicode的编码规则总结出来的数值.
        Int32 wl = 0xDC00; //結果的後16位元初始值,这个地方应该是根据Unicode的编码规则总结出来的数值.
        wh = wh | Vh;
        wl = wl | Vl;
        //Response.Write("wh:"); Response.Write(Convert.ToString(wh, 2)); Response.Write("<br/>");//2进制显示
        //Response.Write("wl:"); Response.Write(Convert.ToString(wl, 2)); Response.Write("<br/>");//2进制显示
        if (LowHeight)
        {
            return wl << 16 | wh;   //低位左移16位以后再把高位合并起来 得到的结果是UTF16的编码值   //适合低位在前的操作系统 
        }
        else
        {
            return wh << 16 | wl; //高位左移16位以后再把低位合并起来 得到的结果是UTF16的编码值   //适合高位在前的操作系统
        }


    }
    public static string EmojiCodeToUTF16String(string EmojiCode)
    {
        if (EmojiCode.Length != 4 && EmojiCode.Length != 5)
        {
            throw new ArgumentException("错误的 EmojiCode 16进制数据长度.一般为4位或5位");
        }
        //1f604
        int EmojiUnicodeHex = int.Parse(EmojiCode, System.Globalization.NumberStyles.HexNumber);

        //1f604对应 utf16 编码的int
        Int32 EmojiUTF16Hex = EmojiToUTF16(EmojiUnicodeHex, true);             //这里字符的低位在前.高位在后.
                                                                               //Response.Write(Convert.ToString(lon, 16)); Response.Write("<br/>"); //这里字符的低位在前.高位在后. 
        var emojiBytes = BitConverter.GetBytes(EmojiUTF16Hex);                     //把整型值变成Byte[]形式. Int64的话 丢掉高位的空白0000000   

        return Encoding.Unicode.GetString(emojiBytes);
    }
    public static int Int(object o)
    {
        return Convert.ToInt32(o);
    }

    public static float Float(object o)
    {
        return (float)Math.Round(Convert.ToSingle(o), 2);
    }

    public static long Long(object o)
    {
        return Convert.ToInt64(o);
    }

    public static int Random(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static string Uid(string uid)
    {
        int position = uid.LastIndexOf('_');
        return uid.Remove(0, position + 1);
    }

    public static long GetTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    public static T Add<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            T[] ts = go.GetComponents<T>();
            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i] != null) GameObject.Destroy(ts[i]);
            }
            return go.gameObject.AddComponent<T>();
        }
        return null;
    }

    /// <summary>
    /// 添加组件
    /// </summary> 
    public static T Add<T>(Transform go) where T : Component
    {
        return Add<T>(go.gameObject);
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(GameObject go, string subnode)
    {
        return Child(go.transform, subnode);
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    public static GameObject Child(Transform go, string subnode)
    {
        Transform tran = go.Find(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(GameObject go, string subnode)
    {
        return Peer(go.transform, subnode);
    }

    /// <summary>
    /// 取平级对象
    /// </summary>
    public static GameObject Peer(Transform go, string subnode)
    {
        Transform tran = go.parent.Find(subnode);
        if (tran == null) return null;
        return tran.gameObject;
    }

    /// <summary>
    /// 手机震动
    /// </summary>
    public static void Vibrate()
    {
        //int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
        //if (canVibrate == 1) iPhoneUtils.Vibrate();
    }

    /// <summary>
    /// Base64编码
    /// </summary>
    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    public static string Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    /// <summary>
    /// 判断数字
    /// </summary>
    public static bool IsNumeric(string str)
    {
        if (str == null || str.Length == 0) return false;
        for (int i = 0; i < str.Length; i++)
        {
            if (!Char.IsNumber(str[i])) { return false; }
        }
        return true;
    }

    /// <summary>
    /// HashToMD5Hex
    /// </summary>
    public static string HashToMD5Hex(string sourceStr)
    {
        byte[] Bytes = Encoding.UTF8.GetBytes(sourceStr);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] result = md5.ComputeHash(Bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                builder.Append(result[i].ToString("x2"));
            return builder.ToString();
        }
    }

    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            string size = fs.Length / 1024 + "";
            Debug.Log("当前文件的大小：  " + file + "===>" + (fs.Length / 1024) + "KB");
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb + "|" + size;
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    public static Dictionary<string, Dictionary<string, string>> ExplainString(string strLine)
    {
        Dictionary<string, string[]> content = new Dictionary<string, string[]>();
        string[] lineArray = strLine.Replace("\r\n", "*").Split(new char[] { '*' });
        //获取行数
        int rows = lineArray.Length - 1;
        //获取列数
        int Columns = lineArray[0].Split(new char[] { '\t' }).Length;
        //定义一个数组用于存放字段名
        string[] ColumnName = new string[Columns];
        for (int i = 0; i < rows; i++)
        {
            string[] Array = lineArray[i].Split(new char[] { '\t' });
            for (int j = 0; j < Columns; j++)
            {
                //获取Array的列的值
                string nvalue = Array[j].Trim();
                if (i == 0)
                {
                    //存储字段名
                    ColumnName[j] = nvalue;
                    content[ColumnName[j]] = new string[rows - 1];
                }
                else
                {
                    //存储对应字段的默认值//<字段名，默认值>
                    content[ColumnName[j]][i - 1] = nvalue;
                }
            }
        }
        //开始解析
        return ExplainDictionary(content);
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainDictionary(Dictionary<string, string[]> content)
    {
        Dictionary<string, Dictionary<string, string>> DicContent = new Dictionary<string, Dictionary<string, string>>();
        //获取字典中所有的键(字段名)
        Dictionary<string, string[]>.KeyCollection Allkeys = content.Keys;
        //遍历所有的字段名
        foreach (string key in Allkeys)
        {
            //实例化一个hasData的字典//<ID,字段值>
            Dictionary<string, string> hasData = new Dictionary<string, string>();
            string[] Data = content[key];
            for (int i = 0; i < Data.Length; i++)
            {
                //<ID><字段值>
                hasData[content["ID"][i]] = Data[i];
            }
            DicContent[key] = hasData;
        }
        return DicContent;
    }
    /// <summary>
    /// 清除所有子节点
    /// </summary>
    public static void ClearChild(Transform go)
    {
        if (go == null) return;
        for (int i = go.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(go.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 是否为数字
    /// </summary>
    public static bool IsNumber(string strNumber)
    {
        Regex regex = new Regex("[^0-9]");
        return !regex.IsMatch(strNumber);
    }

    /// <summary>
    /// 取得行文本
    /// </summary>
    public static string GetFileText(string path)
    {
        return File.ReadAllText(path);
    }

    /// <summary>
    /// 网络可用
    /// </summary>
    public static bool NetAvailable
    {
        get
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }

    /// <summary>
    /// 是否是无线
    /// </summary>
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }


    /// <summary>
    /// 是不是苹果平台
    /// </summary>
    /// <returns></returns>
    public static bool isApplePlatform
    {
        get
        {
            return Application.platform == RuntimePlatform.IPhonePlayer ||
                   Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer;
        }

    }
    public static int RandmoRangeInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    /// <summary>
    /// 加密/解密
    /// </summary>
    /// <param name="targetData">文件流</param>
    public static void Encypt(ref byte[] targetData, byte m_key)
    {
        //加密，与key异或，解密的时候同样如此
        int dataLength = targetData.Length;
        for (int i = 0; i < dataLength; ++i)
        {
            targetData[i] = (byte)(targetData[i] ^ m_key);
        }
    }
}
