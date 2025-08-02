using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Mushroom : MonoBehaviour
{
    [Header("Growth Settings")]
    [Tooltip("How long (in seconds) it takes to grow from zero to full size.")]
    [SerializeField] private float growthDuration = 2f;

    private Vector3 targetScale;
    private ParticleSystem sporeSystem;

    private void Awake()
    {
        // Cache full scale and PS
        targetScale = transform.localScale;
        sporeSystem = GetComponent<ParticleSystem>();

        // Start tiny
        transform.localScale = Vector3.zero;
        // Begin growth+spores
        StartCoroutine(GrowAndRelease());
    }

    private IEnumerator GrowAndRelease()
    {
        float timer = 0f;

        // 1) Grow over time
        while (timer < growthDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / growthDuration);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;

        // 2) Release spores
        if (sporeSystem != null)
            sporeSystem.Play();
        else
            Debug.LogWarning("[Mushroom] No ParticleSystem found to release spores.");
    }
}
