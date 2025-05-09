using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private EnemyStatsHolder statsHolder;
    private Animator anim;
    private EnemyHealth healthComponent;
    private EnemyMovement movement;
    private EnemyCombat combat;
    private EnemyRangedCombat rangedCombat;

    public bool shouldRespawn = true;

    void Awake()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        anim = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
        healthComponent = GetComponent<EnemyHealth>();
        combat = GetComponent<EnemyCombat>();
        rangedCombat = GetComponent<EnemyRangedCombat>();
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    public void HandleDeath()
    {
        if (statsHolder.isDead) return;
            statsHolder.isDead = true;

        DisableComponents();
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        movement.StopMoving();
        DisableComponents();
        anim.SetTrigger("DieTemp");
        if (shouldRespawn)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            shouldRespawn = false;
            statsHolder.isDead = false;


            anim.SetTrigger("Respawn");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 2f);
            healthComponent.ResetHealth();
            EnableComponents();
            movement.StartMoving();
        }
        else
        {
            yield return new WaitForSeconds(0);
            anim.SetTrigger("DiePerm");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        movement.StopMoving();
        healthComponent.isInvulnerable = true;
        DisableComponents();
        anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        GetComponent<EnemyHealth>().ResetHealth();

        movement.StartMoving();
        healthComponent.isInvulnerable = false;
        movement.StartMoving();
        EnableComponents();
    }

    private void DisableComponents()
    {
        if (combat != null) combat.enabled = false;
        if (rangedCombat != null) rangedCombat.enabled = false;
    }

    private void EnableComponents()
    {
        if (combat != null) combat.enabled = true;
        if (rangedCombat != null) rangedCombat.enabled = true;
    }
}