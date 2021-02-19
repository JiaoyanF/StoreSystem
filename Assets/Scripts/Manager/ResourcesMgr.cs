using UnityEngine;
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
    public GameObject Asset { private set; get; }
    public UIMgr UIMgr { private set; get; }
    public string UIName { private set; get; }
    public object[] Args { private set; get; }
    public UI ui;

    public override void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// 实例化资源
    /// </summary>
    public void LoadAsset(UIMgr ui_mgr, string ui_name, params object[] args)
    {
        GameObject goObj = LoadResource<GameObject>(ui_name);
        Asset = GameObject.Instantiate<GameObject>(goObj);
        Asset.name = ui_name;
        if (Asset == null)
        {
            Debug.LogError(GetType() + "克隆资源不成功，path = " + SysDefine.PrefabPath + ui_name);
            return;
        }
        Type type = Type.GetType(ui_name + "UI");
        this.UIMgr = ui_mgr;
        this.UIName = ui_name;
        this.Args = args;
        this.ui = Asset.gameObject.AddComponent(type) as UI;// 挂载脚本
        ui.gameObject.transform.SetParent(UIMgr.Root.transform);// 设置父节点
        ui.InitData(this);
    }

    /// <summary>
    /// 获取资源
    /// </summary>
    public T LoadResource<T>(string path) where T : UnityEngine.Object
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
        ht.Add(path, TResource);
        return TResource;
    }
}