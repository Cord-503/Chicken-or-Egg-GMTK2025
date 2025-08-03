using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [Header("Timeline")]
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private bool pauseTimelineDuringDialogue = true;   

    public void OnSignalRecieved()
    {
        PauseTimeline();
    }
    public void OnContinueButtonClicked() { ResumeTimeline(); }
    private void PauseTimeline()
    {
        if (timelineDirector != null)
            timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    private void ResumeTimeline()
    {
        if (timelineDirector != null)
            timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
