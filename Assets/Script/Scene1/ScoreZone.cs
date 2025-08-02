using UnityEngine;
using UnityEngine.Events;

public class ScoreZone : MonoBehaviour
{
    [Header("Score Settings")]
    public int scorePerBranch = 10;

    [Header("Events")]
    public static UnityEvent<int> OnGlobalScoreUpdated = new UnityEvent<int>();

    [Header("Floating Score UI")]
    public GameObject floatingScorePrefab;

    private static int totalScore = 0; // 静态变量，所有实例共享
    public static int TotalScore => totalScore;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("branch"))
        {
            AddScore(scorePerBranch);

            if (floatingScorePrefab != null)
            {
                Vector3 spawnPos = other.transform.position + Vector3.up * 0.5f;
                GameObject instance = Instantiate(floatingScorePrefab, spawnPos, Quaternion.identity);
                instance.GetComponent<FloatingScoreUI>().SetScore(scorePerBranch);
            }

            StartCoroutine(FadeAndDestroy(other.gameObject));
        }
    }

    public static void AddScore(int amount)
    {
        totalScore += amount;
        OnGlobalScoreUpdated?.Invoke(totalScore);
    }

    public static void AddBonusScore(int bonus) => AddScore(bonus);
    public static void ResetScore()
    {
        totalScore = 0;
        OnGlobalScoreUpdated?.Invoke(totalScore);
    }

    private System.Collections.IEnumerator FadeAndDestroy(GameObject target)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            float duration = 1f;
            float elapsed = 0f;
            Color originalColor = sr.color;

            while (elapsed < duration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        Destroy(target);
    }
}