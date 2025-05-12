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
    private GameObject damageAura;

    void Start()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        currentHealth = statsHolder.maxHealth;
        UpdateHealthBar();
        damageAura = GameObject.Find("Smoke aura");
        if (damageAura != null)
            damageAura.SetActive(false);
    }

    public void TakeDamage(float amount) // Changed parameter to float
    {
        if (isInvulnerable) return;
        damageAura.SetActive(true);
        currentHealth -= amount;
        UpdateHealthBar();
        if (currentHealth <= 0)
            GetComponent<EnemyDeath>().HandleDeath();
            
        StartCoroutine(TakingDamage());
    }

    IEnumerator TakingDamage()
    {
        yield return new WaitForSeconds(0.7f); 
        damageAura.SetActive(false);
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
