using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    int health;

    public event Action OnDamage;
    public event Action OnDie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if (health <= 0) OnDie?.Invoke();
        health = Mathf.Max(health - damage, 0);

        OnDamage?.Invoke();

        Debug.Log("Gameobject: " + gameObject.name + ", Health: " + health);
    }
}
