using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Def;
using System.Reflection;

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
            go = FindChild(child, goName);
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
    public static T FindChild<T>(Transform trans, string goName) where T : Component
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

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件</typeparam>
    /// <param name="gameObject">目标对象</param>
    /// <returns>获取或增加的组件</returns>
    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 获取 GameObject 是否在场景中。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>GameObject 是否在场景中。</returns>
    /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
    public static bool InScene(GameObject gameObject)
    {
        return gameObject.scene.name != null;
    }
}

/// <summary>
/// 事件管理器的工具
/// </summary>
public class UniqueIndex
{
    private int[] indexes_ = null;
    private int alloc_index_ = 0;
    private int count_ = 0;

    public UniqueIndex(int count)
    {
        this.indexes_ = new int[count];
        this.count_ = count;
        this.alloc_index_ = count_;

        for (int i = 0; i < count_; i++)
            this.indexes_[i] = count_ - 1 - i;// 小的放后面,从0开始分配
    }

    public void Grow(int n)
    {
        Array.Resize(ref indexes_, count_ + n);
        for (int i = 0; i < n; i++)
            indexes_[alloc_index_ + i] = count_ + i;
        count_ += n;
        alloc_index_ += n;
    }

    public void RemoveAll()
    {
        alloc_index_ = 0;
    }

    public int Alloc()
    {
        if (alloc_index_ > 0)
            return indexes_[--alloc_index_];
        else
            return -1;
    }

    public void Free(int index)
    {
        if (alloc_index_ < indexes_.Length)
            indexes_[alloc_index_++] = index;
    }

    public bool CanAlloc()
    {
        return alloc_index_ > 0;
    }

    public int CanAllocCount()
    {
        return alloc_index_;
    }

    public void Print()
    {
        Console.WriteLine("alloc_index:{0} ", alloc_index_);
        for (int i = 0; i < alloc_index_; i++)
            Console.WriteLine("  {0}", indexes_[i]);
    }

    public int Count
    {
        get { return count_; }
    }
}

/// <summary>
/// 更好的输出
/// </summary>
public class Log
{
    public static void Debug(string con, params object[] args)
    {
        if (args == null) { return; }
        if (args.Length == 0)
        {
            UnityEngine.Debug.Log(con);
        }
        else
        {
            ArrayList list = new ArrayList();
            foreach (var item in args)
            {
                list.Add(item);
            }
            string[] str = (string[])list.ToArray(typeof(string));
            UnityEngine.Debug.Log(con + " = " + String.Join(",", str));
        }
    }
    public static void Format(string con, params object[] args)
    {
        string str = "";
        foreach (var item in args)
        {
            str = String.Format(con, item.ToString());
        }
        Log.Debug(str);
    }
}
// 本地信息
public class Localization
{
    private static Map<string, string> Lang = new Map<string, string>();
    public static void LoadLang()
    {
        string[] strs = File.ReadAllLines(SysDefine.language);
        foreach (var line in strs)
        {
            string[] str = line.Split(new char[] { '\t' });
            Lang.Add(str[0], str[1]);
        }
    }
    public static string Format(string str)
    {
        return Lang[str];
    }
    public static string Format(string str, params string[] args)
    {
        str = Lang[str];
        if (string.IsNullOrEmpty(str) || args == null || args.Length == 0)
            return string.Empty;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(str, args);
        return sb.ToString();
    }
}