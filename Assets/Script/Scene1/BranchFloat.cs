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
        // ����Ư��
        float sway = Mathf.Sin(Time.time * swayFrequency + swayOffset) * swayAmplitude;
        transform.position += Vector3.right * sway * Time.deltaTime;

        // �Ƕ����ذڶ����Ӱ�ʽ��
        float rotation = Mathf.Sin(Time.time * rotationFrequency + swayOffset) * rotationAmplitude;
        transform.rotation = Quaternion.Euler(0f, 0f, initialRotationZ + rotation);
    }
}
