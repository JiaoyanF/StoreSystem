using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 资源加载管理器
/// 功能：在Unity的Resources类的基础之上，增加了“缓存”的处理。
/// </summary>
public class ResourcesMgr : Obj
{
    private Hashtable ht = null;// 容器键值对集合

    public override void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// 实例化资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isCatch"></param>
    /// <returns></returns>
    public GameObject LoadAsset(string path, bool isCatch = true)
    {
        GameObject goObj = LoadResource<GameObject>(path, isCatch);
        GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj);
        if (goObjClone == null)
        {
            Debug.LogError(GetType() + "克隆资源不成功，path = " + SysDefine.PrefabPath + path);
        }
        return goObjClone;
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="isCatch"></param>
    /// <returns></returns>
    public T LoadResource<T>(string path, bool isCatch) where T : UnityEngine.Object
    {
        if (ht.Contains(path))
        {
            return ht[path] as T;
        }
        T TResource = Resources.Load<T>(SysDefine.PrefabPath + path);
        if (TResource == null)
        {
            Debug.LogError(GetType() + "资源找不到，path = " + path);
        }
        else if (isCatch)
        {
            ht.Add(path, TResource);
        }
        return TResource;
    }
}