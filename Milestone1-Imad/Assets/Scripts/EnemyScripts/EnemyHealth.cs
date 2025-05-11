using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStatsHolder statsHolder;
    public Image healthBarForeground;

    public float currentHealth; // Changed to float
    public bool isInvulnerable = false;

    void Start()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        currentHealth = statsHolder.maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float amount) // Changed parameter to float
    {
        if (isInvulnerable) return;
        currentHealth -= amount;
        UpdateHealthBar();
        if (currentHealth <= 0)
            GetComponent<EnemyDeath>().HandleDeath();
    }

    private void UpdateHealthBar()
    {
        if (healthBarForeground != null)
            healthBarForeground.fillAmount = currentHealth / statsHolder.maxHealth; // No cast needed as both are floats
    }

    public void ResetHealth()
    {
        currentHealth = statsHolder.maxHealth;
        UpdateHealthBar();
    }
}
