using UnityEngine;

public class Dandelion : MonoBehaviour
{
    public float explosionForce = 3f;
    public float windForce = 0.5f;
    public float floatDuration = 3f;

    private bool hasExploded = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasExploded)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    Explode();
                    hasExploded = true;
                }
            }
        }
    }

    void Explode()
    {
        GameObject[] branches = GameObject.FindGameObjectsWithTag("branch");
        foreach (GameObject branch in branches)
        {
            Rigidbody rb = branch.GetComponent<Rigidbody>();
            if (rb == null)
                rb = branch.AddComponent<Rigidbody>();

            rb.useGravity = false;

            Vector3 explosionDir = (branch.transform.position - transform.position).normalized + Random.insideUnitSphere * 0.3f;
            rb.AddForce(explosionDir * explosionForce, ForceMode.Impulse);

            StartCoroutine(ApplyWind(rb));
        }
    }

    System.Collections.IEnumerator ApplyWind(Rigidbody rb)
    {
        float timer = 0f;
        Vector3 windDir = new Vector3(1f, 1f, 0.5f).normalized;

        while (timer < floatDuration)
        {
            rb.AddForce(windDir * windForce, ForceMode.Force);
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rb.gameObject);
    }
}
