using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    public Target CurrentTarget { get; private set; }

    PlayerStateMachine stateMachine;
    Camera mainCamera;
    List<Target> targets = new List<Target>();

    void Awake()
    {
        stateMachine = GetComponentInParent<PlayerStateMachine>();
        mainCamera = Camera.main;
    }

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
            RemoveTarget(target);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0)
        {
            CurrentTarget = null;
            return false;
        }

        Target closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Target target in targets)
        {
            Vector2 viewPosition = mainCamera.WorldToViewportPoint(target.transform.position);

            if (viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1)
            {
                continue;
            }

            Vector2 toCenter = viewPosition - new Vector2(0.5f, 0.5f);
            if (toCenter.magnitude < closestDistance)
            {
                closestDistance = toCenter.magnitude;
                closestTarget = target;
            }
        }

        if (closestTarget == null) return false;

        CurrentTarget = closestTarget;
        targetGroup.RemoveMember(CurrentTarget.transform);
        targetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
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
