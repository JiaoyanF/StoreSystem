
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Def;
using LitJson;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 关于
/// </summary> 
public class AboutUI : UI
{
    public override UILayer Layer { get { return UILayer.Popup; } }
    protected override void Initialize() { }
    protected override void RegEvents()
    {
        SetBtnEvent(Get(this, "mask"), delegate () { Close(); });
        SetBtnEvent(Get(this, "Ensure"), delegate () { Close(); });
    }
    protected override void OnEnable() { }
    protected override void OnUpdate() { }
    protected override void OnDisable() { }
    protected override void OnDestroy() { }
}