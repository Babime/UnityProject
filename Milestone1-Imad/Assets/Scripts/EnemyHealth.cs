using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private EnemyStatsHolder statsHolder;
    public Image healthBarForeground;

    public int currentHealth;
    public bool isInvulnerable = false;

    void Start()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        currentHealth = statsHolder.maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
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
            healthBarForeground.fillAmount = (float)currentHealth / statsHolder.maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = statsHolder.maxHealth;
        UpdateHealthBar();
    }


}