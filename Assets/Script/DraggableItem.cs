// DraggableItem.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ItemType { PlasticBag, ToothBrush, Orange, Meat, CD, MilkJar, Comb, SodaCan, Leaf, Coin, Homework, Cellphone }

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType itemType;
    public Image    icon;
    private CanvasGroup canvasGroup;
    private RectTransform rect;
    private Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        startPos = rect.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        rect.anchoredPosition += e.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        canvasGroup.blocksRaycasts = true;
        // if not dropped on a drop zone, snap back
        rect.anchoredPosition = startPos;
    }
}
