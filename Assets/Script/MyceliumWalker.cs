using UnityEngine;
using System.Collections;

public class MyceliumWalker : MonoBehaviour
{
    [Header("Walker Settings")]
    public int walkers = 20;
    public float stepLength = 0.2f;
    [Range(0,1)] public float branchChance = 0.05f;
    public float stepInterval = 0.1f;
    
    [Header("Soil Boundary")]
    public float horizonY = 0f;               // y-level of the soil surface

    [Header("Rendering")]
    public GameObject lineSegmentPrefab;      // prefab with a LineRenderer


    /// <summary>
    /// Flip to true once CenterMycelium() has run.
    /// </summary>
    public bool IsReady { get; private set; }

    public event System.Action OnFullyGrown;
    public Vector3 center { get; private set; }

    // internal tracking
    private int totalWalkers, stepsPerWalker;
    private int remainingWalkers;
    private bool hasFiredEvent;


    /// <summary>
    /// Call this to start a batch of walkers.
    /// </summary>
    public void Generate(int walkerCount, int stepsEach)
    {
        totalWalkers     
            = remainingWalkers
            = walkerCount;
        stepsPerWalker   = stepsEach;
        hasFiredEvent    = false;

        for (int i = 0; i < walkerCount; i++)
            StartCoroutine(SpawnWalker());
    }

    /// <summary>
    /// Feed the fungus to get more growth.
    /// </summary>
    public void ContinueGrowth(int moreWalkers, int moreStepsPerWalker)
    {
        totalWalkers += moreWalkers;
        remainingWalkers += moreWalkers;

        // you could choose to override stepsPerWalker or keep the old one
        stepsPerWalker = moreStepsPerWalker;

        for (int i = 0; i < moreWalkers; i++)
            StartCoroutine(SpawnWalker());

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

            yield return new WaitForSeconds(stepInterval);
        }

        remainingWalkers--;

        if (remainingWalkers == 0 && !hasFiredEvent)
        {
            hasFiredEvent = true;
            CenterMycelium();
            IsReady = true;
            OnFullyGrown?.Invoke();
        }
    }

    /// <summary>
    /// Destroys all generated line segments and resets internal flags.
    /// </summary>
    public void Clear()
    {
        // destroy every child segment (each segment is one GameObject)
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        // reset state so Generate(...) will work again
        IsReady        = false;
        hasFiredEvent  = false;
        remainingWalkers = 0;
        totalWalkers     = 0;
        // (center will get recomputed when you next Generate)
    }

    void CenterMycelium()
    {
        var lines = GetComponentsInChildren<LineRenderer>();
        if (lines.Length == 0) return;

        Vector3 min = Vector3.positiveInfinity, max = Vector3.negativeInfinity;
        foreach (var lr in lines)
            for (int i = 0; i < 2; i++)
            {
                Vector3 p = lr.GetPosition(i);
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }

        // compute midpoint
        Vector3 c = (min + max) * 0.5f;

        // lock Y and Z to the walker’s own transform so only X offset remains
        c.y = 0;
        c.z = 0;

        // now assign to the property
        center = c;
    }

}
