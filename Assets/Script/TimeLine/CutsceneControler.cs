using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneControler : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;


    void Start()
    {
        if (director != null)
        {
            director.stopped += OnTimelineStopped;
        }
    }

    void OnTimelineStopped(PlayableDirector obj)
    {
        SceneManager.LoadScene("Dandelion Game");
    }
}
