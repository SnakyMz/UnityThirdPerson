using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Collider gameObjectCollider;
    int damage = 0;
    float knockback = 0;

    List<Collider> alreadyCollided = new();

    void OnEnable()
    {
        alreadyCollided.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == gameObjectCollider || alreadyCollided.Contains(other)) return;

        alreadyCollided.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
            if (other.TryGetComponent<ForceReceiver>(out ForceReceiver character))
            {
                Vector3 impact = (other.transform.position - gameObjectCollider.transform.position).normalized * knockback;
                character.AddImpact(impact);
            }
        }
    }

    public void SetAttack(int damageAmount, float knockbackAmount)
    {
        damage = damageAmount;
        knockback = knockbackAmount;
    }
}
