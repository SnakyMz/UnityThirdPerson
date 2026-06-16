using UnityEngine;
using System;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;

    void OnTriggerEnter(Collider other)
    {
        OnLedgeDetect?.Invoke(other.ClosestPoint(transform.position), other.transform.forward);
    }
}
