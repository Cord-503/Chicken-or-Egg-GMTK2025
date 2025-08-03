using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneControler : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;


    private void OnCutsceneEnd(PlayableDirector director)
    {
        SceneManager.LoadScene("Dandelion Game");

    }

}
