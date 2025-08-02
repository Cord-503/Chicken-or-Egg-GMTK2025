using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MushroomSpawner2D : MonoBehaviour
{
    [Header("Prefab & Points")]
    public GameObject mushroomPrefab;
    public GameObject myceliumPrefab;
    public Transform[] spawnPoints;

    [Header("Timing")]
    public float initialDelay = 5f;      // seconds to wait before spawning mushrooms

    [Header("Variation")]
    public Vector2 scaleRange     = new Vector2(0.8f, 1.2f);
    public float   myceliumScale  = 0.5f;
    public float   hueOffset      = 0.1f;
    
    [Header("Auto Play")]
    public bool autoStart = false;

    void Start()
    {
        if (mushroomPrefab == null || myceliumPrefab == null 
            || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Assign all prefabs and spawn points!");
            return;
        }
        if (autoStart) {
            StartCoroutine(SpawnAllAfterDelay());
        }
    }

    public void SpawnAt(Vector3 worldPos)
    {
        Instantiate(
          mushroomPrefab,
          worldPos,
          Quaternion.identity
        );
    }

    /// <summary>
    /// Public entry–point: call this to kick off the spawn coroutine.
    /// </summary>
    public void StartSpawning()
    {
        StartCoroutine(SpawnAllAfterDelay());
    }

    private IEnumerator SpawnAllAfterDelay()
    {
        // 1) Spawn all mycelium and collect their walker scripts
        var walkers = new List<MyceliumWalker>();
        foreach (var pt in spawnPoints)
        {
            var mObj = Instantiate(myceliumPrefab, pt.position, Quaternion.identity);
            mObj.transform.localScale *= myceliumScale;
            
            // FIXED: proper generic GetComponent<T>() call
            var walker = mObj.GetComponent<MyceliumWalker>();
            walker.Generate(20, 10);
            walkers.Add(walker);
        }

        // 2) Wait until every MyceliumWalker is Ready
        yield return new WaitUntil(() => walkers.TrueForAll(w => w.IsReady));

        // 3) Record their computed centers
        var centers = walkers.Select(w => w.center).ToList();

        // 4) (Optional) wait before mushrooms pop
        yield return new WaitForSeconds(initialDelay);

        // 5) Spawn mushrooms at each center
        for (int i = 0; i < centers.Count; i++)
        {
            var worldPos = centers[i] * myceliumScale + spawnPoints[i].position;
            var m = Instantiate(mushroomPrefab, worldPos, Quaternion.identity);

            // random scale
            float s = Random.Range(scaleRange.x, scaleRange.y);
            m.transform.localScale *= s;

            // hue‐shift the cap
            var capRend = m.transform.Find("Cap2D")
                              ?.GetComponent<SpriteRenderer>();
            if (capRend != null)
            {
                Color.RGBToHSV(capRend.color, out float h, out float sat, out float val);
                h += Random.Range(-hueOffset, hueOffset);
                capRend.color = Color.HSVToRGB(h, sat, val);
            }
        }
    }
}
