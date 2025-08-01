using UnityEngine;
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

    void Start()
    {
        if (mushroomPrefab == null || myceliumPrefab == null 
            || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Assign all prefabs and spawn points!");
            return;
        }
        StartCoroutine(SpawnAllAfterDelay());
    }

    IEnumerator SpawnAllAfterDelay()
    {
        // 1) Spawn all mycelium and collect their walker scripts
        var walkers = new List<MyceliumWalker>();
        foreach (var pt in spawnPoints)
        {
            var mObj = Instantiate(myceliumPrefab, pt.position, Quaternion.identity);
            mObj.transform.localScale *= myceliumScale;
            walkers.Add(mObj.GetComponent<MyceliumWalker>());
        }

        // 2) Wait until every MyceliumWalker is Ready
        yield return new WaitUntil(() => walkers.TrueForAll(w => w.IsReady));

        // 3) Record their computed centers
        var centers = new List<Vector3>();
        foreach (var w in walkers)
            centers.Add(w.center);

        // 4) (Optional) wait before mushrooms pop
        yield return new WaitForSeconds(initialDelay);

        // 5) Spawn mushrooms at each center
        for (int i = 0; i < centers.Count; i++)
        {
            var m = Instantiate(mushroomPrefab, centers[i]*myceliumScale + spawnPoints[i].position, Quaternion.identity);

            // random scale
            float s = Random.Range(scaleRange.x, scaleRange.y);
            
            m.transform.localScale *= s;

            // hue‐shift the cap
            var capRend = m.transform.Find("Cap2D")
                              ?.GetComponent<SpriteRenderer>();
            if (capRend != null)
            {
                Color baseCol = capRend.color;
                float h, sat, val;
                Color.RGBToHSV(baseCol, out h, out sat, out val);
                h += Random.Range(-hueOffset, hueOffset);
                capRend.color = Color.HSVToRGB(h, sat, val);
            }
        }
    }
}
