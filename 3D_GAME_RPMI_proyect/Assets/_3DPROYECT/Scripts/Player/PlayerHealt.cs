using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    public float health;

    public Action<float> OnHealthChanged;

    bool isDead = false; //  importante

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return; //  evita daño después de morir

        health -= dmg;

        if (health < 0)
            health = 0;

        Debug.Log("Player hit! HP: " + health);

        float healthPercent = health / maxHealth;
        OnHealthChanged?.Invoke(healthPercent);

        if (health <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    void Die()
    {

        // Opcional: parar el tiempo
        Time.timeScale = 1f;

        // Cargar escena de muerte
        SceneManager.LoadScene(4);
    }
}