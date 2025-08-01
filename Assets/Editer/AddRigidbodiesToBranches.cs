using UnityEngine;
using UnityEditor;

public class AddRigidbodiesToBranches
{
    [MenuItem("Tools/Add Rigidbody2D and Collider2D to All Branches")]
    public static void AddComponents()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("branch");
        int count = 0;

        foreach (GameObject branch in branches)
        {
            if (branch == null) continue;

            //if (branch.GetComponent<Collider2D>() == null)
            //{
            //    branch.AddComponent<PolygonCollider2D>();
            //}

            Rigidbody2D rb = branch.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = branch.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                count++;
            }
        }

        Debug.Log($"✅ 成功为 {count} 个 branch 添加 Rigidbody2D 和 Collider2D！");
    }
}
