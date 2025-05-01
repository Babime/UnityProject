using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private EnemyStatsHolder statsHolder;
    private Animator anim;
    private bool isDying = false;
    private EnemyHealth healthComponent;
    private EnemyMovement movement;

    void Awake()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        healthComponent = GetComponent<EnemyHealth>();
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void HandleDeath()
    {
        if (isDying) return;
        isDying = true;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        movement.StopMoving();
        anim.SetTrigger("DieTemp");
        yield return new WaitForSeconds(2f);

        bool shouldRespawn = true;
        var combat = GetComponent<EnemyCombat>();

        if (shouldRespawn)
        {
            if (combat != null)
                combat.enabled = false;
            anim.SetTrigger("Respawn");
            yield return new WaitForSeconds(statsHolder.spawnDuration);
            isDying = false;
            healthComponent.ResetHealth();
            movement.StartMoving();
            combat.enabled = true;

        }
        else
        {
            if (combat != null)
                combat.enabled = false;
            anim.SetTrigger("DiePerm");
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
            combat.enabled = true;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        var combat = GetComponent<EnemyCombat>();
        if (combat != null)
            combat.enabled = false;
        healthComponent.isInvulnerable = true;
        movement.StopMoving();
        anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(statsHolder.spawnDuration);

        if (isDying)
            GetComponent<EnemyHealth>().ResetHealth();

        movement.StartMoving();
        healthComponent.isInvulnerable = false;
        combat.enabled = true;
    }
}