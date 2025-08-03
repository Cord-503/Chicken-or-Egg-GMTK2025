// MyceliumDropZone.cs
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MyceliumDropZone : MonoBehaviour, IDropHandler
{
    public MushroomGameManager gameManager;

    public void OnDrop(PointerEventData e)
    {
        var item = e.pointerDrag?.GetComponent<DraggableItem>();
        
        if (item != null)
        {
            gameManager.HandleDrop(item);
        }
    }
}
