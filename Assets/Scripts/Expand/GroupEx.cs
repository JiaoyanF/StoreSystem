using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
 
 /// <summary>
 /// 组
 /// </summary>
public class GroupEx : MonoBehaviour
{
    Map<GameObject, bool> Childs = new Map<GameObject, bool>();// <子元素，状态>
    void Start()
    {
    }
    void OnEnable()
    {
    }
    void Update()
    {
    }
    /// <summary>
    /// 子元素选中变化
    /// </summary>
    /// <param name="child"></param>
    /// <param name="is_select"></param>
    public void ChildChange(GameObject child, bool is_select)
    {
        if (Childs.ContainsKey(child))
        {
            Childs[child] = is_select;
        }else
        {
            Childs.Add(child, is_select);
        }
    }
    /// <summary>
    ///  返回是否有子元素被选中状态
    /// </summary>
    /// <returns></returns>
    public bool GetSelectChildState()
    {
        return Childs.Count > 0 ? Childs.ContainsValue(true) : false;
    }
}