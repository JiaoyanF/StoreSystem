using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录面板
/// </summary> 
public class LoginPanel : UIElement
{
    InputField un_input;
    InputField ps_input;

    protected override void Initialize()
    {
        un_input = GetControl<InputField>(Get(this, "un_input"));
        ps_input = GetControl<InputField>(Get(this, "ps_input"));
    }
    protected override void RegEvents()
    {
        // base.RegEvents();
        SetBtnEvent(Get(this, "mask"), MaskClick);
        SetBtnEvent(Get(this, "login"), LoadData);
        SetBtnEvent(Get(this, "reset"), ResetData);
    }
    protected override void OnEnable()
    {
    }
    protected override void OnUpdata()
    {
    }
    protected override void OnDisable()
    {
    }
    protected override void OnDestroy()
    {
    }
    /// <summary>
    /// 登录加载
    /// </summary> 
    private void LoadData()
    {
        // 判断账户密码正确
        if (un_input.text == "000" && ps_input.text == "000")
        {
            ResetData();
            Log.Debug("成功登录");
            FireEvent(new Events.UI.OpenUI("MainForm"));
        }
        else
        {
            Log.Debug("登录失败");
            un_input.text = "000";
            ps_input.text = "000";
        }
    }
    /// <summary>
    /// 重置
    /// </summary> 
    private void ResetData()
    {
        un_input.text = "";
        ps_input.text = "";
    }
    private void MaskClick()
    {
        Log.Debug("重置数据，关闭登录面板");
        ResetData();
        (Root as StartUI).ChangeBtnsShow();
        this.gameObject.SetActive(false);
    }
}