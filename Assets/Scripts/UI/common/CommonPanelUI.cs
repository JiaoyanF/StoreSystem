using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using DG.Tweening;
using LitJson;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 公共面板
/// </summary> 
public class CommonPanelUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    private Text Con;// 提示
    private BtnItem BtnModule;// 按钮原型
    private List<BtnItem> Btns = new List<BtnItem>();// 按钮组
    protected override void Initialize()
    {
        Con = GetControl<Text>(this, "context");
        BtnModule = NewElement<BtnItem>(this, Get(this, "Button"));
    }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
    }
    protected override void OnEnable()
    {
        if (context.Length > 0)
            Con.text = context[0].ToString();
        Map<string, Action> btns = context[1] as Map<string, Action>;
        foreach (var item in btns)
        {
            BtnItem btn = BtnModule.Clone<BtnItem>();
            btn.RefreshData(item.Key, item.Value);
            Btns.Add(btn);
        }
    }
    protected override void OnUpdate()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void OnDestroy()
    {
    }
}
class BtnItem : UIElement
{
    Text tex;
    Action ClickFunc;
    protected override void Initialize()
    {
        tex = Root.GetControl<Text>(this, "Text");
    }
    protected override void RegEvents()
    {
        Root.SetBtnEvent(this.gameObject, OnClick);
    }
    public void RefreshData(string name, Action func = null)
    {
        tex.text = name;
        ClickFunc = func;
    }
    public void OnClick()
    {
        if (ClickFunc != null)
            ClickFunc();
        Root.Close();
    }
}