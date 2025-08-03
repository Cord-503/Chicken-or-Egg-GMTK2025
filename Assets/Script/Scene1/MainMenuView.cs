using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Animator fadeInOutBlack_Anim;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private float delay = 2.0f;
 

  //  private SceneManager sceneManager;
    private Dandelion dandelion;

    public void OnNewGameClicked()
    {
        StartCoroutine(DelayAndLoadScene(delay));
    }

    IEnumerator DelayAndLoadScene(float d)
    {
        yield return new WaitForSeconds(d);
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("IntroduceDandelion");
    }

    public void OnCreditClicked()
    {
        StartCoroutine(DelayAndLoadCredit());
    }
    IEnumerator DelayAndLoadCredit()
    {
        yield return new WaitForSeconds(0.5f);
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Credit");
    }

}
