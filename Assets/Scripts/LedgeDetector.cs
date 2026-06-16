using UnityEngine;
using System;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3> OnLedgeDetect;

    void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke(other.transform.forward);
    }
}
