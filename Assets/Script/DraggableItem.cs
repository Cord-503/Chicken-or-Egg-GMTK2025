// DraggableItem.cs
using UnityEngine;
using UnityEngine.EventSystems;

public enum ItemType { Apple, Rock, SodaCan, Wood }

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemType itemType;
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
