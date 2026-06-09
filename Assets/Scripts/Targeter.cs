using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    public Target CurrentTarget { get; private set; }

    List<Target> targets = new List<Target>();

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            target.OnDestroyed += RemoveTarget;
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

    public void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            CurrentTarget = null;
            targetGroup.RemoveMember(target.transform);
        }
        targets.Remove(target);
        target.OnDestroyed -= RemoveTarget;
    }
}
