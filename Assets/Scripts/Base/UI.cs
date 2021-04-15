using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Def;

/// <summary>
/// ui基类
/// </summary> 
public abstract class UI : MonoBehaviour, IDisposable
{
    private Canvas Canvas;
    private int PlaneDistance = 2;// 距摄像机平面距离
    public virtual UILayer Layer { get; }// ui层
    public UIMgr ui_mgr;
    public object[] context; //加载时传入的数据
    public string Name { get; private set; }// ui名
    private GameObject Asset;
    public int HashID { get; private set; }
    public NetMgr NetMgr { get { return ui_mgr.NetMgr; } }
    protected List<EventMgr.Handler> event_handlers = new List<EventMgr.Handler>();

    public void InitData(ResourcesMgr res)
    {
        this.Asset = res.Asset;
        this.Name = res.UIName;
        this.ui_mgr = res.UIMgr;
        this.context = res.Args;
        this.Canvas = GetControl<Canvas>(this.gameObject);

        CanvasControl();
        Show();
    }
    /// <summary>
    /// 画布控制
    /// </summary>
    private void CanvasControl()
    {
        if (this.Canvas == null)
        {
            Log.Debug("缺少画布组件");
            return;
        }
        this.Canvas.worldCamera = this.ui_mgr.UICamera;
        this.Canvas.planeDistance = this.PlaneDistance;
        this.Canvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (!this.Canvas.worldCamera)
            this.Canvas.worldCamera = this.ui_mgr.UICamera;
        switch (Layer)
        {
            case UILayer.Normal:
                this.Canvas.sortingOrder = 1;
                break;
            case UILayer.Full:
                this.Canvas.sortingOrder = 2;
                break;
            case UILayer.Popup:
                this.Canvas.sortingOrder = 3;
                break;
            case UILayer.Tips:
                this.Canvas.sortingOrder = 4;
                break;
            default:
                this.Canvas.sortingOrder = 0;
                break;
        }
        this.Canvas.pixelPerfect = false;
    }

    #region UI状态

    protected void Awake()
    {
        HashID = this.GetHashCode();
        Initialize();
        RegEvents();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Initialize() { }
    /// <summary>
    /// 注册事件
    /// </summary>
    protected virtual void RegEvents() { }
    protected virtual void Start() { }
    /// <summary>
    /// 打开面板
    /// </summary>
    protected virtual void OnEnable() { }
    protected void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// 帧刷新
    /// </summary>
    protected virtual void OnUpdate() { }
    /// <summary>
    /// 关闭面板
    /// </summary>
    protected virtual void OnDisable() { }
    /// <summary>
    /// 销毁面板
    /// </summary>
    protected virtual void OnDestroy() { }
    public void Dispose()
    {
        if (event_handlers == null) return;
        for (int i = 0; i < event_handlers.Count; i++)
        {
            UnRegEventHandler(event_handlers[i]);
        }
        event_handlers.Clear();
        event_handlers = null;
        Destroy(Asset);
    }

    public void Show(object[] Args = null)
    {
        if (Args != null)
            this.context = Args;
        ui_mgr.ShowUI(this);
        // 面板为活跃状态时刷新界面
        SetActive(this, false);
        SetActive(this, true);
    }
    /// <summary>
    /// 关闭面板
    /// </summary>
    public void Close()
    {
        ui_mgr.CloseUI(this);
        SetActive(this, false);
        Dispose();
    }
    public void SetActive<T>(T obj, bool active) where T : Component
    {
        obj.gameObject.SetActive(active);
    }
    public void SetActive(GameObject obj, bool active)
    {
        obj.SetActive(active);
    }

    #endregion

    #region 公共方法

    public virtual void FireEvent(Event e)
    {
        this.ui_mgr.FireEvent(e);
    }
    public EventMgr.Handler RegEventHandler<T>(EventMgr.OnEventRecv<T> recv) where T : Event
    {
        EventMgr.Handler h = this.ui_mgr.system_mgr.RegEventHandler<T>(recv);
        if (event_handlers == null)
            return null;
        this.event_handlers.Add(h);
        return h;
    }
    public void UnRegEventHandler(EventMgr.Handler h)
    {
        if (h == null || this.event_handlers == null || this.event_handlers.Count == 0)
            return;
        if (this.event_handlers.Contains(h))
            this.ui_mgr.system_mgr.UnregEventHandler(h);
        this.event_handlers.Remove(h);
    }

    /// <summary>
    /// 创建UI子脚本
    /// </summary>
    /// <param name="root">主UI</param>
    /// <param name="obj">子对象</param>
    /// <typeparam name="T">子对象类</typeparam>
    /// <returns></returns>
    public T NewElement<T>(UI root, GameObject obj) where T : UIElement
    {
        obj.SetActive(false);
        T ele = Tool.GetOrAddComponent<T>(obj);
        ele.Root = root;
        return ele;
    }

    /// <summary>
    /// 更改背景图片
    /// </summary>
    /// <param name="img_name"></param>
    public void ChangeBG(string img_name)
    {
        Texture2D tex = ui_mgr.ResMgr.LoadResource<Texture2D>(SysDefine.RawImagePath + img_name);
        ui_mgr.BG.texture = tex;
    }

    /// <summary>
    /// 设置背景显示与否
    /// </summary>
    /// <param name="is_show"></param>
    public void ShowBG(bool is_show)
    {
        SetActive(ui_mgr.BG.transform.parent, is_show);
    }

    /// <summary>
    /// 获取物体
    /// </summary> 
    /// <param name="own">父</param>
    /// <param name="target">目标</param>
    public GameObject Get<T>(T own, string target) where T : Component
    {
        return Tool.FindChild(own.transform, target).gameObject;
    }
    public GameObject Get(GameObject own, string target)
    {
        return Tool.FindChild(own.transform, target).gameObject;
    }

    /// <summary>
    /// 获取root子物体有T控件的物体组件列表
    /// </summary>
    /// <param name="root"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> GetComponentChild<T>(GameObject root) where T : Component
    {
        List<T> list = new List<T>();
        foreach (Transform item in root.transform)
        {
            T conn = GetControl<T>(item.gameObject);
            if (conn != null)
            {
                list.Add(conn);
            }
        }
        return list;
    }

    /// <summary>
    /// 获取物体控件
    /// </summary> 
    /// <param name="own">父</param>
    /// <param name="target">目标名</param>
    public T GetControl<T>(Component own, string target) where T : Component
    {
        return GetControl<T>(Tool.FindChild(own.transform, target).gameObject);
    }
    public T GetControl<T>(GameObject own, string target) where T : Component
    {
        return GetControl<T>(Tool.FindChild(own.transform, target).gameObject);
    }
    public T GetControl<T>(GameObject own) where T : Component
    {
        return own.GetComponent<T>();
    }

    /// <summary>
    /// 设置按钮点击事件
    /// </summary> 
    /// <param name="btn">物体</param>
    /// <param name="func">执行方法</param>
    public void SetBtnEvent(GameObject btn, UnityAction func)
    {
        btn.GetComponent<Button>().onClick.AddListener(func);
    }
    public void SetBtnEvent(Button btn, UnityAction func)
    {
        btn.onClick.AddListener(func);
    }

    /// <summary>
    /// 设置文本内容
    /// </summary> 
    /// <param name="obj">物体</param>
    /// <param name="str">执行方法</param>
    public void SetText<T>(GameObject obj, object str) where T : Component
    {
        (obj.GetComponent<T>() as Text).text = str.ToString();
    }

    #endregion
}

/// <summary>
/// ui元素基类
/// </summary>
public abstract class UIElement : MonoBehaviour, IDisposable
{
    public UI Root;// 根节点
    public int HashID { get; private set; }
    private bool initialized = false;
    private object[] context; //加载时传入的数据
    protected void Awake()
    {
        HashID = this.GetHashCode();
        DoInitIfDont();
    }
    protected virtual void Initialize() { }
    protected virtual void RegEvents() { }
    protected virtual void Start() { }
    protected virtual void OnEnable() { }
    protected void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }
    public void Dispose()
    {
        Root.SetActive(this, false);
        Destroy(this.gameObject);
    }
    internal void DoInitIfDont()
    {
        if (!initialized)
        {
            Initialize();
            RegEvents();
            initialized = true;
        }
    }
    public void Show(bool active)
    {
        this.enabled = active;
        if (this.gameObject.activeInHierarchy != active)
            this.gameObject.SetActive(active);
    }
    /// <summary>
    /// 实例化克隆物体
    /// </summary>
    /// <param name="root"></param>
    /// <param name="sample"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Clone<T>(object[] Args = null) where T : UIElement
    {
        if (!this.gameObject || !this.transform.parent)
            return null;
        this.gameObject.SetActive(false);
        GameObject item = CreateChild(this.transform.parent.gameObject, this.gameObject);
        if (!item)
            return null;
        // DoInitIfDont();
        // item.SetActive(false);
        T ele = Tool.GetOrAddComponent<T>(item);
        ele.Root = Root;
        ele.context = Args;
        ele.Show(true);
        return ele;
    }
    public List<T> Clone<T>(List<T> list, int count) where T : UIElement
    {
        if (count <= 0)
            return null;
        foreach (var item in list)
        {
            item.Dispose();
        }
        list = new List<T>();
        for (int i = 0; i < count; i++)
        {
            list.Add(this.Clone<T>());
        }
        return list;
    }

    /// <summary>
    /// 创建子对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="sample"></param>
    /// <returns></returns>
    public GameObject CreateChild(GameObject parent, GameObject sample)
    {
        GameObject item = GameObject.Instantiate<GameObject>(sample);
        if (parent && item)
        {
            Transform t = item.transform;
            t.SetParent(parent.transform);// 设置父节点
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            item.layer = parent.layer;
        }
        return item;
    }
}