using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class CountDownManager : MonoBehaviour
{
  
    [SerializeField] private float countdownTime = 25f; // 倒计时秒数
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Animator fadeInOutBlack_Anim;
    [SerializeField] private GameObject scoreCounter;
    
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
        

        // 你可以在这里触发场景切换、开始游戏等
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


