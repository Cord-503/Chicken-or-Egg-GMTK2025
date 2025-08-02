using UnityEngine;

public class BranchSpawner : MonoBehaviour
{
    [Header("Branch Settings")]
    public GameObject branchPrefab;
    public float spawnInterval = 2f;
    public int maxBranches = 20;

    [Header("Movement Settings")]
    public Vector2 horizontalDriftRange = new Vector2(-1f, 1f);
    public Vector2 verticalSpeedRange = new Vector2(-1f, -2f);

    private int currentBranchCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBranch), 0f, spawnInterval);
    }

    void SpawnBranch()
    {
        if (currentBranchCount >= maxBranches) return;

        Vector3 spawnPosition = transform.position;

        GameObject branch = Instantiate(branchPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = branch.GetComponent<Rigidbody2D>();
        if (rb == null) rb = branch.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(
            Random.Range(horizontalDriftRange.x, horizontalDriftRange.y),
            Random.Range(verticalSpeedRange.x, verticalSpeedRange.y)
        );

        currentBranchCount++;
    }
}
