using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    public MushroomSpawner2D spawner;

    public void Replay()
    {
        // 1) Destroy every line‐segment (each segment is its own GameObject with a LineRenderer)
        var segments = FindObjectsOfType<LineRenderer>();
        foreach (var lr in segments)
        {
            Destroy(lr.gameObject);
        }

        // 2) Destroy any spawned mushrooms
        foreach (var m in GameObject.FindGameObjectsWithTag("Mushroom"))
        {
            Destroy(m);
        }

        // 3) Start spawning again
        spawner.StartSpawning();
    }
}
