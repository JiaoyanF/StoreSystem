using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// ui基类
/// </summary> 
public abstract class UI : MonoBehaviour
{
    public UIMgr ui_mgr;
    public object[] context; //加载时传入的数据

    protected void Awake()
    {
        
    }

    protected void Updata()
    {
        OnUpdata();
    }
    public virtual void Start(){}
    public virtual void OnEnable(){}
    public virtual void OnUpdata(){}
    public virtual void OnDisable(){}
    public virtual void OnDestroy(){}

    #region UI窗体的四种状态

    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void ActiveTrue()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 重新显示状态
    /// </summary>
    public virtual void ReActiveTrue()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 冻结状态
    /// </summary>
    public virtual void Freeze()
    {
        gameObject.SetActive(true);
    }

    #endregion

    #region 封装子类常用方法
    void Open(string ui_name)
    {

    }

    void Close()
    {

    }

    #endregion

    #region 公共方法

    public void FireEvent(Event e)
    {
        this.ui_mgr.FireEvent(e);
    }

    /// <summary>
    /// 获取物体
    /// </summary> 
    /// <param name="own">父</param>
    /// <param name="target">目标</param>
    public GameObject Get<T>(T own, string target) where T : UI
    {
        return Tool.FindChild(own.transform, target).gameObject;
    }
    public GameObject Get(GameObject own, string target)
    {
        return Tool.FindChild(own.transform, target).gameObject;
    }

    /// <summary>
    /// 获取物体控件
    /// </summary> 
    /// <param name="own">父</param>
    /// <param name="target">目标名</param>
    public T GetControl<T, U>(U own, string target) 
    where U : UI, new()
    where T : Component
    {
        return Tool.FindChild(own.transform, target).gameObject.GetComponent<T>();
    }
    public T GetControl<T>(GameObject own) where T : Component
    {
        return own.gameObject.GetComponent<T>();
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

    /// <summary>
    /// 设置文本内容
    /// </summary> 
    /// <param name="obj">物体</param>
    /// <param name="str">执行方法</param>
    public void SetText<T>(GameObject obj, string str) where T : Component
    {
        (obj.GetComponent<T>() as Text).text = str;
    }
    public void SetText<T>(T tex, string str) where T : Component
    {
        (tex as Text).text = str;
    }

    #endregion
}