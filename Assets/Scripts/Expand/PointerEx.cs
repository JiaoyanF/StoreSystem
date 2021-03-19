using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
 /// <summary>
 /// 鼠标事件拓展
 /// </summary>
public class PointerEx : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }
    void OnEnable()
    {
        SetImageShow(false);
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
    }
}