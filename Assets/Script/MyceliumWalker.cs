using UnityEngine;
using System.Collections;

public class MyceliumWalker : MonoBehaviour
{
    [Header("Walker Settings")]
    public int walkers = 20;
    public int stepsPerWalker = 50;
    public float stepLength = 0.2f;
    [Range(0,1)] public float branchChance = 0.05f;

    [Header("Soil Boundary")]
    public float horizonY = 0f;               // y-level of the soil surface

    [Header("Rendering")]
    public GameObject lineSegmentPrefab;      // prefab with a LineRenderer

    // Tracks how many walker-coroutines are still “alive”
    private int remainingWalkers;

    /// <summary>
    /// The computed visual center in world coords.  Valid only once IsReady == true.
    /// </summary>
    public Vector3 center;

    /// <summary>
    /// Flip to true once CenterMycelium() has run.
    /// </summary>
    public bool IsReady { get; private set; }

    IEnumerator Start()
    {
        remainingWalkers = walkers;
        for (int i = 0; i < walkers; i++)
            StartCoroutine(SpawnWalker());

        // wait until every branch and walker is done
        yield return new WaitUntil(() => remainingWalkers == 0);

        CenterMycelium();
        IsReady = true;   // signal to spawner that center is valid
    }

    IEnumerator SpawnWalker()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Min(pos.y, horizonY - 0.01f);
        Vector3 dir = Random.insideUnitCircle.normalized;

        for (int i = 0; i < stepsPerWalker; i++)
        {
            dir = Quaternion.Euler(0,0,Random.Range(-30f,30f)) * dir;
            Vector3 newPos = pos + dir * stepLength;

            if (newPos.y > horizonY)
            {
                newPos.y = horizonY;
                dir.y = -Mathf.Abs(dir.y);
            }

            var seg = Instantiate(lineSegmentPrefab, transform);
            var lr  = seg.GetComponent<LineRenderer>();
            lr.SetPosition(0, pos);
            lr.SetPosition(1, newPos);

            pos = newPos;

            if (Random.value < branchChance)
            {
                remainingWalkers++;
                StartCoroutine(SpawnWalker());
            }

            yield return null;
        }

        remainingWalkers--;
    }

    void CenterMycelium()
    {
        var lines = GetComponentsInChildren<LineRenderer>();
        if (lines.Length == 0) return;
        Debug.Log("lines: " + lines.Length);
        Vector3 min = Vector3.positiveInfinity, max = Vector3.negativeInfinity;
        foreach (var lr in lines)
            for (int i = 0; i < 2; i++)
            {
                var p = lr.GetPosition(i);
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        // world‐space midpoint of the cloud
        center = (min + max) * 0.5f;
        // lock Y and Z to the prefab’s transform so we only use the X‐offset
        center.y = 0;
        center.z = 0;
    }
}
