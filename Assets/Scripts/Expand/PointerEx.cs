using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
 
 /// <summary>
 /// 鼠标事件拓展
 /// </summary>
public class PointerEx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private bool is_curr = false;// 当前是否是选中的
    private UnityAction ClickFunc;// 点击回调
    public GameObject RootObj;// 父组
    private GroupEx Group;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void OnEnable()
    {
        SetImageShow(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (is_curr && ClickFunc != null)
                ClickFunc();
            if (RootObj && is_curr || Group.GetSelectChildState() == false)
                RootObj.SetActive(false);
        }
    }
    /// <summary>
    /// 鼠标移入事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetImageShow(true);
    }
    /// <summary>
    /// 鼠标移出事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        SetImageShow(false);
    }
    private void SetImageShow(bool show)
    {
        if (image)
        {
            image.enabled = show;
        }
        is_curr = show;
        if (Group)
            Group.ChildChange(this.gameObject, is_curr);
    }
    /// <summary>
    /// 设置事件
    /// </summary>
    /// <param name="func"></param>
    /// <param name="root"></param>
    public void SetEvent(UnityAction func, GameObject root = null)
    {
        ClickFunc = func;
        RootObj = root;
        if (RootObj)
            Group = Tool.GetOrAddComponent<GroupEx>(RootObj);
    }
}