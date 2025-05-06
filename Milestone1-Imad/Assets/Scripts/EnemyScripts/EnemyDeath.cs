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

        movement.enabled = false;
        if (combat != null) combat.enabled = false;
        if (rangedCombat != null) rangedCombat.enabled = false;
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        movement.StopMoving();
        anim.SetTrigger("DieTemp");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        bool shouldRespawn = true;

        if (shouldRespawn)
        {
            statsHolder.isDead = false;
            movement.enabled = false;
            if (combat != null) combat.enabled = false;
            if (rangedCombat != null) rangedCombat.enabled = false;

            anim.SetTrigger("Respawn");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + 2f);
            healthComponent.ResetHealth();
            movement.enabled = true;
            if (combat != null) combat.enabled = true;
            if (rangedCombat != null) rangedCombat.enabled = true;
        }
        else
        {
            if (combat != null) combat.enabled = false;
            if (rangedCombat != null) rangedCombat.enabled = false;
            anim.SetTrigger("DiePerm");
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
            if (combat != null) combat.enabled = true;
            if (rangedCombat != null) rangedCombat.enabled = true;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        if (combat != null) combat.enabled = false;
        healthComponent.isInvulnerable = true;
        if (rangedCombat != null) rangedCombat.enabled = false;
        movement.StopMoving();
        anim.SetTrigger("Spawn");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        GetComponent<EnemyHealth>().ResetHealth();

        movement.StartMoving();
        healthComponent.isInvulnerable = false;
        if (combat != null) combat.enabled = true;
        if (rangedCombat != null) rangedCombat.enabled = true;
    }
}