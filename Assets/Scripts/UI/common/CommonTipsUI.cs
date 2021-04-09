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
/// 公共提示
/// </summary> 
public class CommonTipsUI : UI
{
    public override UILayer Layer { get { return UILayer.Tips; } }
    private GameObject Con;// 提示
    protected override void Initialize()
    {
        Con = Get(this, "conent");
        // GetControl<Text>(Con).color = 255;
        // InvokeRepeating("RefreshPox", 0.0f,0.2f);
    }
    /// <summary>
    /// Tweening插件的移动
    /// </summary>
    private void RefreshPox()
    {
        if (Con.transform.localPosition.y < 140)
            Close();
        // Con.transform.DOLocalMoveY(Con.transform.localPosition.y - 20, 0.3f);
        Con.transform.Translate(new Vector3(0, Time.deltaTime * -1, 0));
    }
    protected override void RegEvents()
    {
    }
    protected override void OnEnable()
    {
        Log.Debug(context[0].ToString());
        if (context.Length > 0)
            SetText<Text>(Con, context[0].ToString());
    }
    protected override void OnUpdate()
    {
        RefreshPox();
    }
    protected override void OnDisable()
    {
        // CancelInvoke("RefreshPox");
    }
    protected override void OnDestroy()
    {
    }
}