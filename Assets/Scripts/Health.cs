using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    int health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (health <= 0) return;
        health = Mathf.Max(health - damage, 0);

        Debug.Log(health);
    }
}
