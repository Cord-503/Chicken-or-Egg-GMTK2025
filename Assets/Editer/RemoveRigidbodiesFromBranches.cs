using UnityEngine;
using UnityEditor;

public class RemoveRigidbodiesFromBranches
{
    [MenuItem("Tools/Remove Rigidbody and Collider from All Branches")]
    public static void RemoveComponents()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("branch");
        int count = 0;

        foreach (GameObject branch in branches)
        {
            if (branch == null) continue;

            Rigidbody rb = branch.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Object.DestroyImmediate(rb);
                count++;
            }

            Collider col = branch.GetComponent<Collider>();
            if (col != null)
            {
                Object.DestroyImmediate(col);
            }
        }

        Debug.Log($"🧹 已从 {count} 个 branch 中移除 Rigidbody 和 Collider。");
    }
}
