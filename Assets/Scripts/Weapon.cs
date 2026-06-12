using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Collider playerCollider;
    int damage = 0;

    List<Collider> alreadyCollided = new();

    void OnEnable()
    {
        alreadyCollided.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider || alreadyCollided.Contains(other)) return;

        alreadyCollided.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }
    }

    public void SetAttackDamage(int amount)
    {
        damage = amount;
    }
}
