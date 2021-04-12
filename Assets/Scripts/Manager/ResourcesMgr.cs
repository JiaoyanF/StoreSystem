using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Def;

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
        GameObject goObj = LoadPrefab<GameObject>(ui_name);
        Asset = GameObject.Instantiate<GameObject>(goObj);
        Asset.name = ui_name;
        if (Asset == null)
        {
            Debug.LogError(GetType() + "克隆资源不成功，path = " + SysDefine.PrefabPath + ui_name);
            return;
        }
        // 为了避免OnEnable先于InitDta执行↓
        Asset.gameObject.SetActive(false);

        Type type = Type.GetType(ui_name + "UI");
        this.UIMgr = ui_mgr;
        this.UIName = ui_name;
        this.Args = args;
        this.ui = Asset.gameObject.AddComponent(type) as UI;// 挂载脚本
        ui.gameObject.transform.SetParent(UIMgr.Root.transform);// 设置父节点
        ui.InitData(this);
    }

    public T LoadAsset<T>(SystemMgr sys_mgr, string name) where T : MonoBehaviour
    {
        GameObject goObj = LoadPrefab<GameObject>(name);
        Asset = GameObject.Instantiate<GameObject>(goObj);
        Asset.name = name;
        if (Asset == null)
        {
            Debug.LogError(GetType() + "克隆资源不成功，path = " + SysDefine.PrefabPath + name);
            return null;
        }

        Type type = Type.GetType(name);
        MonoBehaviour mono = Asset.gameObject.AddComponent(type) as MonoBehaviour;// 挂载脚本
        UnityEngine.Object.DontDestroyOnLoad(mono.transform);// 切换场景不销毁
        return mono as T;
    }

    /// <summary>
    /// 获取预制体资源
    /// </summary>
    public T LoadPrefab<T>(string prefab_name) where T : UnityEngine.Object
    {
        if (ht.Contains(prefab_name))
        {
            return ht[prefab_name] as T;
        }
        T TResource = LoadResource<T>(SysDefine.PrefabPath + prefab_name);
        ht.Add(prefab_name, TResource);
        return TResource;
    }
    public T LoadResource<T>(string path)where T : UnityEngine.Object
    {
        T ass = Resources.Load<T>(path);
        if (ass == null)
        {
            Debug.LogError(GetType() + "资源找不到，path = " + path);
        }
        return ass;
    }
}