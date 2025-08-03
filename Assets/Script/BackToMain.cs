using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMain : MonoBehaviour
{

    [SerializeField] private Animator fadeInOutBlack_Anim = null;
    [SerializeField] private float delay = 2.0f;
    [SerializeField] private string sceneToLoad;
    public void OnNewGameClicked()
    {
        StartCoroutine(DelayAndLoadScene(delay));
    }

    IEnumerator DelayAndLoadScene(float d)
    {
        yield return new WaitForSeconds(d);
        fadeInOutBlack_Anim.SetBool("FadeOut", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
