using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    public Target CurrentTarget { get; private set; }

    List<Target> targets = new List<Target>();

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Remove(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0)
        {
            CurrentTarget = null;
            return false;
        }

        CurrentTarget = targets[0];
        return true;
    }
}
