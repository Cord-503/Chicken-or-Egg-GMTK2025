// GameManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

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

    [SerializeField] private Animator fadeInOutBlack_Anim;
    [SerializeField] private float delay = 3.0f;
    [SerializeField] private TextMeshProUGUI instructionText;


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
            instructionText.text = "Nice choices. Your mycelium has grown!";



            mycelium.ContinueGrowth(20, 5 + 3 * currentStage);

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
            // 加入游戏结束判断
            if (feedCount >= 4)
            {
                GameOver();
                return;
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

    private void GameOver()
    {

        // 可选：停止一切进一步交互
        hasFinishedGrowth = true;
        instructionText.text = "Congratulations, your mycelium has grown into a mushroom!";
        // TODO: 添加你想做的游戏结束逻辑，比如切换场景、弹出UI、统计得分等
        StartCoroutine(DelayAndLoadScene(delay));
    }

    IEnumerator DelayAndLoadScene(float d)
    {
        yield return new WaitForSeconds(d);
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EndScene");
    }
}


