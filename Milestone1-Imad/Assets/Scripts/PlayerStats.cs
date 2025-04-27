using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float maxHP = 100f;
    public float maxStamina = 100f;
    public float attackDamage = 15f;
    public float attackSpeed = 1f; // attacks per second
    public float moveSpeed = 5f;

    [Header("Regeneration Rates")]
    public float hpRegenPerSecond = 2f;
    public float staminaRegenPerSecond = 10f;

    public float currentHP { get; private set; }
    public float currentStamina { get; private set; }
    public bool isAlive { get; private set; } = true;

    [Header("UI Elements")]
    public HealthBar healthBar; 
    public StaminaBar staminaBar; 

    [Header("Player Controller Reference")]
    public PlayerController playerController; 

    private void Awake()
    {
        currentHP = maxHP;
        currentStamina = maxStamina;
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHP);
        if (staminaBar != null)
            staminaBar.SetMaxStamina(maxStamina);
    }

    private void Update()
    {
        if (isAlive)
        {
            RegenerateStats();
        }
    }

    private void RegenerateStats()
    {
        if (currentHP < maxHP)
        {
            Heal(hpRegenPerSecond * Time.deltaTime);
        }

        if (currentStamina < maxStamina)
        {
            RecoverStamina(staminaRegenPerSecond * Time.deltaTime);
        }

        healthBar.SetHealth(currentHP);
        staminaBar.SetStamina(currentStamina);
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHP -= amount;
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            Die();
        }
    }

    public bool SpendStamina(float amount)
    {
        if (!isAlive || currentStamina < amount)
            return false;

        currentStamina -= amount;
        return true;
    }

    public void Heal(float amount)
    {
        if (!isAlive) return;

        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void RecoverStamina(float amount)
    {
        if (!isAlive) return;

        currentStamina += amount;
        if (currentStamina > maxStamina)
            currentStamina = maxStamina;
    }

    private void Die()
    {
        isAlive = false;
        playerController.Die(); 
    }
}
