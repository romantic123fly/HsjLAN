#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.UI;

//Spine 2d动画工具
//using Spine.Unity;

/// <summary>
/// 1、定义一个静态类
/// 2、静态类的名称和要实现扩展方法的具体类无关
/// // 3、实现一个具体的静态方法
// 4、第一个参数必须使用this关键字指定要使用扩展方法的类型
/// </summary>
/// 


//自定义的工具函数类 
public static class GameExtensions
{
    
    public static RectTransform rectTransform(this Component cp)
    {
        return cp.transform as RectTransform;
    }

    public static void setLocalPositionX(this Transform value, float x)
    {
        var pos = value.localPosition;
        pos.x = x;
        value.localPosition = pos;
    }
    public static void setLocalPositionVector3(this Transform value, Vector3 pos)
    {
        value.localPosition = pos;
    }

    public static void setPositionX(this Transform value, float x)
    {
        var pos = value.position;
        pos.x = x;
        value.position = pos;
    }

    public static void setPositionY(this Transform value, float y)
    {
        var pos = value.position;
        pos.y = y;
        value.position = pos;
    }

    public static void setPositionZ(this Transform value, float z)
    {
        var pos = value.position;
        pos.z = z;
        value.position = pos;
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // string转int,这里需要注意Convert.ToInt32是四舍五入,遇到0.5小数值时,会被转为偶数,如1.5变成2
    public static int ToInt(this string str)
    {
        int num;
        bool isNum = Int32.TryParse(str, out num);
        if(!isNum)
        {
            UnityEngine.Debug.LogWarning(str + " ToInt illegal");
            num = 0;
        }
        return num;
    }

    // float
    public static int ToInt(this float value)
    {
        return (int)value;
    }

    // int转float
    public static float ToFloat(this int value)
    {
        return (float)value;
    }

    public static void AddListener(this UnityEvent value, UnityAction action, object arg0)
    {
        value.AddListener(action);
    }

    public static void setAlpha(this Image value, float alpha)
    {
        var color = value.color;
        color.a = alpha;
        value.color = color;
    }

    /// <summary>
    /// 原始
    /// </summary>
    public static object copy(this object obj)
    {
        object targetDeepCopyObj;

        if (obj == null)
        {
            UnityEngine.Debug.Log("copy obj is null");
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
                        field.SetValue(targetDeepCopyObj, copy(fieldValue));
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
                            myProperty.SetValue(targetDeepCopyObj, copy(propertyValue), null);
                        }
                    }

                }
            }
        }
        return targetDeepCopyObj;
    }

    public static void copy(this object source, object target)
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
                        field.SetValue(target, copy(fieldValue));
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
                            myProperty.SetValue(target, copy(propertyValue), null);
                        }
                    }

                }
            }
        }
    }

    //public static bool FindAnimation(this SkeletonAnimation animation, string animationName)
    //{
    //    return animation.state.Data.skeletonData.FindAnimation(animationName) != null;
    //}
}
