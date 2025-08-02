using UnityEngine;
using System.Collections;

public class Dandelion : MonoBehaviour
{
    [Header("爆炸参数")]
    public float explosionForce = 3f;
    public float explosionRandomness = 0.3f;

    [Header("风力参数")]
    public float windForce = 0.5f;
    public float windForceVariance = 0.2f;
    public float windAngleVariance = 30f;
    public float floatDuration = 4f;
    public float floatDurationVariance = 1f;
    public Vector2 windDirection = new Vector2(1f, 0f);

    private bool hasExploded = false;

    void Start()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("branch");
        foreach (GameObject branch in branches)
        {
            Rigidbody2D rb = branch.GetComponent<Rigidbody2D>();
            if (rb == null)
                rb = branch.AddComponent<Rigidbody2D>();

            rb.gravityScale = 0f;
            rb.simulated = false;
        }
    }

    public void TriggerExplosion()
    {
        if (hasExploded) return;
        Explode();
        hasExploded = true;
    }

    void Explode()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("branch");
        foreach (GameObject branch in branches)
        {
            Rigidbody2D rb = branch.GetComponent<Rigidbody2D>();
            if (rb == null) continue;

            rb.simulated = true;

            Vector2 explosionDir = (Vector2)(branch.transform.position - transform.position).normalized;
            explosionDir += Random.insideUnitCircle * explosionRandomness;

            rb.AddForce(explosionDir.normalized * explosionForce, ForceMode2D.Impulse);

            // 生成每个 branch 独立的风力和风向
            float windMagnitude = windForce + Random.Range(-windForceVariance, windForceVariance);

            float angleOffset = Random.Range(-windAngleVariance, windAngleVariance);
            float baseAngle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            float randomAngle = baseAngle + angleOffset;
            Vector2 randomizedWindDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;

            float duration = floatDuration + Random.Range(-floatDurationVariance, floatDurationVariance);

            StartCoroutine(ApplyWind(rb, randomizedWindDirection, windMagnitude, duration));
        }
    }

    IEnumerator ApplyWind(Rigidbody2D rb, Vector2 windDir, float windStrength, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            rb.AddForce(windDir * windStrength, ForceMode2D.Force);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rb.gameObject);
    }
}
