using UnityEngine;

public class WindController : MonoBehaviour
{
    [Header("Wind Settings")]
    public float windForce = 5f;
    public Vector2 windDirection = Vector2.right;

    private void Update()
    {
        // A 键切换风向为左
        if (Input.GetKeyDown(KeyCode.A))
        {
            windDirection = Vector2.left;
        }

        // D 键切换风向为右
        if (Input.GetKeyDown(KeyCode.D))
        {
            windDirection = Vector2.right;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("branch"))
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                rb.AddForce(windDirection * windForce);
            }
        }
    }
}
