using UnityEngine;

public class FloatingBranch : MonoBehaviour
{
    public float swayAmplitude = 0.5f; 
    public float swayFrequency = 1f; 
    public float rotationAmplitude = 15f;
    public float rotationFrequency = 1f;

    private float swayOffset;
    private float initialRotationZ;

    void Start()
    {
        swayOffset = Random.Range(0f, Mathf.PI * 2f);
        initialRotationZ = transform.eulerAngles.z;
    }

    void Update()
    {
        // 左右漂浮
        float sway = Mathf.Sin(Time.time * swayFrequency + swayOffset) * swayAmplitude;
        transform.position += Vector3.right * sway * Time.deltaTime;

        // 角度来回摆动（钟摆式）
        float rotation = Mathf.Sin(Time.time * rotationFrequency + swayOffset) * rotationAmplitude;
        transform.rotation = Quaternion.Euler(0f, 0f, initialRotationZ + rotation);
    }
}
