using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(Animator))]
public class EnemyRangedCombat : MonoBehaviour
{
    private EnemyStatsHolder stats;
    private EnemyMovement movement;
    public GameObject projectilePrefab;
    private Animator anim;
    private Transform player;
    private float lastFireTime = -Mathf.Infinity;
    private bool isFiring = false;

    void Awake()
    {
        stats    = GetComponent<EnemyStatsHolder>();
        movement = GetComponent<EnemyMovement>();
        anim     = GetComponent<Animator>();
        player   = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        // Debug.Log("isDead: " + stats.isDead);
        if (player == null || stats.isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= stats.attackRange)
        {
            // 2) Tourner doucement vers le joueur
            // Vector3 toPlayer = (player.position - transform.position).normalized;
            // Quaternion targetRot = Quaternion.LookRotation(toPlayer, Vector3.up);
            // transform.rotation = Quaternion.RotateTowards(
            //     transform.rotation,
            //     targetRot,
            //     stats.rotationSpeed * Time.deltaTime
            // );

            // 3) Tirer si le cooldown est passé
            if (!isFiring && Time.time - lastFireTime >= stats.rangedAttackCooldown)
                StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        movement.StopMoving();
        isFiring     = true;
        lastFireTime = Time.time;

        anim.SetTrigger("Cast");

        yield return new WaitForSeconds(stats.fireDelay);

        if (projectilePrefab != null)
        {
            Transform handSlot = transform.Find("Rig/root/hips/spine/chest/upperarm.r/lowerarm.r/wrist.r/hand.r/handslot.r");
            Vector3 spawnPos = handSlot != null ? handSlot.position : transform.position;

            var proj = Instantiate(
                projectilePrefab,
                spawnPos,
                Quaternion.LookRotation(player.position - spawnPos)
            );
            var fb = proj.GetComponent<Fireball>();
            if (fb != null)
            {
                fb.speed = stats.projectileSpeed;
                fb.damage = stats.projectileDamage;
            }
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        isFiring = false;
        movement.StartMoving();
    }
}
