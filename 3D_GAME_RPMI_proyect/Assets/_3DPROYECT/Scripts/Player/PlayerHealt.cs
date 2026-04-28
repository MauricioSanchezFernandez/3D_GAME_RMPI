using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    public float health;

    public Action<float> OnHealthChanged; // <-- evento para la UI

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health < 0)
            health = 0;

        Debug.Log("Player hit! HP: " + health);

        float healthPercent = health / maxHealth;
        OnHealthChanged?.Invoke(healthPercent);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player muerto");
        // aquí puedes meter respawn o pantalla de muerte
    }
}