// GameManager.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MushroomGameManager : MonoBehaviour
{
    [Header("References")]
    public MyceliumWalker  mycelium;
    public MushroomSpawner2D spawner;

    [Header("Game Rules")]
    public int initialWalkers   = 5;
    public int steps     = 20;
    public List<ItemType> edibleItems;

    private bool hasFinishedGrowth = false;
    private int feedCount = 0;
    void Start()
    {
        // start an initial baby mycelium
        mycelium.Generate(initialWalkers, steps);
    }


    public void HandleDrop(DraggableItem item)
    {
        if (hasFinishedGrowth) return;

        if (edibleItems.Contains(item.itemType))
        {
            // feed it—grow a bit more
            feedCount++;
            mycelium.ContinueGrowth(mycelium.walkers, steps * (1 + feedCount));
            
            // fade out the item in place and then destroy it
            StartCoroutine(FadeAndRemove(item));

            // once we’ve given every edible, spawn the mushroom
            if (feedCount >= edibleItems.Count)
            {
                spawner.SpawnAt(mycelium.center);
                hasFinishedGrowth = true;
            }
        }
    }

    private IEnumerator FadeAndRemove(DraggableItem item)
    {
        // disable further dragging
        item.GetComponent<CanvasGroup>().blocksRaycasts = false;

        CanvasGroup cg = item.GetComponent<CanvasGroup>();
        float duration = 1f;
        float elapsed = 0f;
        float start = cg.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, 0f, elapsed / duration);
            yield return null;
        }

        // finally, destroy the UI object
        Destroy(item.gameObject);
    }
}
