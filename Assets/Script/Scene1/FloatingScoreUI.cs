using UnityEngine;
using TMPro;

public class FloatingScoreUI : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;
    public TextMeshProUGUI scoreText;

    private float timer = 0f;
    private Color originalColor;

    void Start()
    {
        if (scoreText == null)
            scoreText = GetComponentInChildren<TextMeshProUGUI>();
        scoreText.text = "+999";
        originalColor = scoreText.color;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColor.a, 0f, timer / fadeDuration);
        scoreText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int amount)
    {
        if (scoreText != null)
        {
            scoreText.text = "+" + amount.ToString();
        }
    }
}
