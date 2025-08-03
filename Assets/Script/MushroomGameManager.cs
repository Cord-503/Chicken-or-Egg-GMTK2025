// GameManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class QuizStage
{
    public ItemType[] options;     // which items to show
    public Sprite[]  sprites;      // matching icons
    public ItemType  correct;      // the one “food” that works
}

public class MushroomGameManager : MonoBehaviour
{
    [Header("Quiz Stages")]
    public QuizStage[] stages;

    [Header("UI")]
    public Transform           inventoryPanel;     // the Panel under your Canvas
    public DraggableItem       optionPrefab; 

    [Header("References")]
    public MyceliumWalker  mycelium;
    public MushroomSpawner2D spawner;

    [Header("Game Rules")]
    public int initialWalkers   = 5;
    public int steps     = 20;
    public List<ItemType> edibleItems;

    private bool hasFinishedGrowth = false;
    private int feedCount = 0;
    private int currentStage = 0;

    void Start()
    {
        // start an initial baby mycelium
        mycelium.Generate(initialWalkers, steps);
        ShowStage(0);
    }

    void ShowStage(int idx)
    {
        // clear old icons
        foreach (Transform t in inventoryPanel) Destroy(t.gameObject);

        var stage = stages[idx];
        for (int i = 0; i < stage.options.Length; i++)
        {
            Debug.Log(optionPrefab);
            Debug.Log(inventoryPanel);
            Debug.Log(stage.options[i]);
            Debug.Log(stage.sprites[i]);
            var opt = Instantiate(optionPrefab, inventoryPanel);
            opt.itemType = stage.options[i];
            opt.icon.sprite = stage.sprites[i];
            var sprite = stage.sprites[i];
            var fitter = opt.GetComponent<AspectRatioFitter>();
            fitter.aspectRatio = (float)sprite.rect.width / sprite.rect.height;
        }
    }


    public void HandleDrop(DraggableItem item)
    {
        if (hasFinishedGrowth) return;

        var stage = stages[currentStage];
        if (item.itemType == stage.correct)
        {
            FadeAndRemove(item);
            // grow it
            feedCount++;
            mycelium.ContinueGrowth(20, 5 + 3*currentStage);

            // advance
            currentStage++;
            if (currentStage < stages.Length)
            {
                ShowStage(currentStage);
            }
            else
            {
                // quiz done
                hasFinishedGrowth = true;
                Destroy(item.gameObject);
                spawner.SpawnAt(mycelium.center + mycelium.transform.position);
            }
        }
    }


    private IEnumerator FadeAndRemove(DraggableItem item)
    {
        // disable further dragging
        item.GetComponent<CanvasGroup>().blocksRaycasts = false;

        CanvasGroup cg = item.GetComponent<CanvasGroup>();
        if (cg == null) yield break;
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
