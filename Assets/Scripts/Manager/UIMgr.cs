using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ui管理器
/// </summary>
public class UIMgr : Obj
{
    /// <summary>
    /// 当前ui
    /// </summary>
    /// <typeparam name="string">ui名称</typeparam>
    /// <typeparam name="UI">ui实例对象</typeparam>
    /// <returns></returns>
    public Map<string, UI> uis = new Map<string, UI>();
    public GameObject RootUI;// 根节点
    public UI ui;
    public ResourcesMgr ResMgr;// 资源管理器

    public override void Awake()
    {
        base.Awake();

        ResMgr = system_mgr.GetSingleT<ResourcesMgr>();

        RootUI = (GameObject)Resources.Load(SysDefine.PrefabPath + "Root");// 获取根节点
        RootUI = UnityEngine.Object.Instantiate(RootUI);
        UnityEngine.Object.DontDestroyOnLoad(RootUI);// 切换场景不销毁
        // RootUI.AddComponent<UIMgr>();
        FireEvent(new Events.UI.OpenUI("StartUI", "00" ,"111"));
    }

    protected override void RegEvents()
    {
        RegEventHandler<Events.UI.OpenUI>(OnCreateUIEvent);
    }

    private void OnCreateUIEvent(Obj sender, Events.UI.OpenUI e)
    {
        Log.Debug("open_ui：" + e.UI);
        UI ui = LoadUI(e.UI, e.Args);
    }

    #region 私有方法

    /// <summary>
    /// 加载ui
    /// </summary>
    /// <param name="UIName"></param>
    /// <returns></returns>
    private UI LoadUI(string UIName, params object[] Args)
    {
        if (uis.ContainsKey(UIName))
        {
            return uis[UIName];
        }
        Type type = Type.GetType(UIName);

        GameObject obj = ResMgr.LoadAsset(UIName);// 加载资源并实例化
        this.ui = obj.gameObject.AddComponent(type) as UI;// 挂载脚本
        this.ui.gameObject.transform.SetParent(RootUI.transform);// 设置父节点

        this.ui.ui_mgr = this;
        this.ui.context = Args;

        uis.Add(UIName, this.ui);
        return this.ui;
    }

    #endregion
}