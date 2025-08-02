using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerDetector : MonoBehaviour
{
    public Dandelion dandelion;

    private void OnMouseDown()
    {
        if (dandelion != null)
        {
            dandelion.TriggerExplosion();
        }
    }
}
