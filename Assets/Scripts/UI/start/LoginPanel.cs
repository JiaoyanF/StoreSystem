using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 登录面板
/// </summary> 
public class LoginPanel : UI
{
    GameObject obj;// 自身
    GameObject mutex;// 互斥对象
    InputField un_input;
    InputField ps_input;

    /// <summary>
    /// 构造
    /// </summary> 
    /// <param name="obj">自身</param>
    /// <param name="mutex">互斥对象</param>
    public LoginPanel(GameObject obj, GameObject mutex)
    {
        this.obj = obj;
        this.mutex = mutex;
        Start();
    }
    public override void Start()
    {
        // Tool.FindChild(this.obj.transform, "mask").gameObject.GetComponent<Button>().onClick.AddListener(MaskClick);
        SetBtnEvent(Get(this.obj, "mask"), MaskClick);
        // Tool.FindChild(this.obj.transform, "login").gameObject.GetComponent<Button>().onClick.AddListener(LoadData);
        GetControl<Button>(Get(this.obj,"login")).onClick.AddListener(LoadData);
        Tool.FindChild(this.obj.transform, "reset").gameObject.GetComponent<Button>().onClick.AddListener(ResetData);
        // un_input = Tool.FindChild(this.obj.transform, "un_input").gameObject.GetComponent<InputField>();
        un_input = GetControl<InputField>(Get(this.obj, "un_input"));
        ps_input = GetControl<InputField>(Get(this.obj, "ps_input"));
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
            //跳转到主界面
        }
        else
        {
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
        print("111111111");
        ResetData();
        this.obj.SetActive(false);
        this.mutex.SetActive(true);
    }
}