using UnityEngine;

public class Dandelion : MonoBehaviour
{
    [Header("爆炸参数")]
    public float explosionForce = 3f;
    public float explosionRandomness = 0.3f;

    [Header("风力参数")]
    public float windForce = 0.5f;
    public Vector2 windDirection = new Vector2(1f, 1f);
    public float floatDuration = 4f;

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

            StartCoroutine(ApplyWind(rb));
        }
    }

    System.Collections.IEnumerator ApplyWind(Rigidbody2D rb)
    {
        float timer = 0f;
        Vector2 windDir = windDirection.normalized;

        while (timer < floatDuration)
        {
            rb.AddForce(windDir * windForce, ForceMode2D.Force);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rb.gameObject);
    }
}
