using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class CountDownManager : MonoBehaviour
{
  
    [SerializeField] private float countdownTime = 25f; // µ¹¼ÆÊ±ÃëÊý
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Animator fadeInOutBlack_Anim;

    
    private float delay = 5.0f;
    private float currentTime;

    void Start()
    {
        currentTime = countdownTime;
        UpdateCountdownDisplay();
        StartCoroutine(CountdownCoroutine());
    }

    System.Collections.IEnumerator CountdownCoroutine()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateCountdownDisplay();
        }

        CountdownFinished();
    }

    void UpdateCountdownDisplay()
    {
        countdownText.text = Mathf.Ceil(currentTime).ToString("0");
    }

    void CountdownFinished()
    {
        countdownText.text = "TIMES UP!";

        GameObject[] scoreZones = GameObject.FindGameObjectsWithTag("ScoreZone");
        foreach (GameObject zone in scoreZones)
        {
            Collider2D col = zone.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        StartCoroutine(DelayAndLoadScene(delay));
    }





    IEnumerator DelayAndLoadScene(float d)
{
    yield return new WaitForSeconds(d);
    fadeInOutBlack_Anim.SetBool("FadeOut", true);
    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene("Mushroom");
}

}


