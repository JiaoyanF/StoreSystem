using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 工具类
/// </summary> 
public class Tool
{
    /// <summary>
    /// 查找子物体（递归查找）  
    /// </summary> 
    /// <param name="trans">父物体</param>
    /// <param name="goName">子物体的名称</param>
    /// <returns>找到的相应子物体</returns>
    public static Transform FindChild(Transform trans, string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
        {
            return child;
        }
        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child,goName);
            if (go != null)
            {
                return go;
            }
        }
        return null;
    }

    /// <summary>
    /// 查找子物体（递归查找）  where T : UnityEngine.Object
    /// </summary> 
    /// <param name="trans">父物体</param>
    /// <param name="goName">子物体的名称</param>
    /// <returns>找到的相应子物体</returns>
    public static T FindChild<T>(Transform trans, string goName) where  T : Object
    {
        Transform child = trans.Find(goName);
        if (child != null)
        {
            return child.GetComponent<T>();
        }
 
        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChild(child, goName);
            if (go != null)
            {
                return go.GetComponent<T>();
            }
        }
        return null;
    }
}
