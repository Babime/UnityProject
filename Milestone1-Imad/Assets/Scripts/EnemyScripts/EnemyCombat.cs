using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(Animator))]
public class EnemyCombat : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject weaponPrefab;
    public Transform handSlot;

    private EnemyStatsHolder stats;
    private EnemyMovement movement; 
    private Animator anim;
    private Transform player;
    private float lastAttackTime = -Mathf.Infinity;
    private bool isAttacking = false;


    void Awake()
    {
        stats = GetComponent<EnemyStatsHolder>();
        movement = GetComponent<EnemyMovement>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player")?.transform;

    }

    void Start()
    {
        if (weaponPrefab != null && handSlot != null)
        {
            var w = Instantiate(weaponPrefab, handSlot);
            w.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            w.AddComponent<WeaponDamageDealer>();
        }
    }

    void Update()
    {
        if (player == null || stats.isDead) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= stats.attackRange)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toPlayer);
            if (angle > stats.attackAngle)
                return;
            
            if (Time.time - lastAttackTime >= stats.attackCooldown && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }

        }
    }

    private IEnumerator AttackRoutine()
    {
        movement.StopMoving();
        isAttacking = true;
        lastAttackTime = Time.time;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 1f);
        movement.StartMoving();
        isAttacking = false;
    }
}
