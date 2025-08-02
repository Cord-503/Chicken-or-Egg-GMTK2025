using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        ScoreZone.OnGlobalScoreUpdated.AddListener(UpdateScoreText);
    }

    void OnDestroy()
    {
        ScoreZone.OnGlobalScoreUpdated.RemoveListener(UpdateScoreText);
    }

    void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
    }
}