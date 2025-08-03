using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    public MushroomSpawner2D spawner;

    public void Replay()
    {

        // 1) Destroy any spawned mushrooms
        if (GameObject.FindGameObjectsWithTag("Mushroom").Length == 0)
        {
            return;
        }
        foreach (var m in GameObject.FindGameObjectsWithTag("Mushroom"))
        {
            Destroy(m);
        }

        // 2) Destroy every line‐segment (each segment is its own GameObject with a LineRenderer)
        var segments = FindObjectsOfType<LineRenderer>();
        foreach (var lr in segments)
        {
            Destroy(lr.gameObject);
        }

        // 3) Start spawning again
        spawner.StartSpawning();
    }
}
