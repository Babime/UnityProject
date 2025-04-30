using UnityEngine;

public class EnemyDamageTester : MonoBehaviour
{
    private EnemyHealth health;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // Touche Q pour infliger 10 de dégâts
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health.TakeDamage(10);
            Debug.Log($"Enemy took 10 damage, remaining HP: {health.currentHealth}");
        }

        // Touche E pour infliger la mort instantanément
        if (Input.GetKeyDown(KeyCode.E))
        {
            health.TakeDamage(int.MaxValue);
            Debug.Log("Enemy should die now!");
        }
    }
}