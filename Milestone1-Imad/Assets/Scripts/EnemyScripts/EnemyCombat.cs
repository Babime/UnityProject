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
        anim     = GetComponent<Animator>();
        player   = GameObject.FindWithTag("Player")?.transform;
    }

    void Start()
    {
        if (weaponPrefab != null && handSlot != null)
        {
            var w = Instantiate(weaponPrefab, handSlot);
            w.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= stats.attackRange)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toPlayer);
            if (angle > stats.attackAngle)
                return;
            movement.StopMoving();
            
            if (Time.time - lastAttackTime >= stats.attackCooldown && !isAttacking)
            {
                Debug.Log("Attack possible");
                StartCoroutine(AttackRoutine());
            }

        }
        else
        {
            movement.StartMoving();
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        movement.StopMoving();

        anim.SetTrigger("Attack");

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float clipLength = state.length;
        yield return new WaitForSeconds(clipLength);

        // 4) Applique les dégâts (si tu veux qu’ils arrivent à la fin)
        // var ph = player.GetComponent<PlayerHealth>();
        // if (ph != null)
        //     ph.TakeDamage(damage);

        // 5) Relance le mouvement et autorise la prochaine attaque
        movement.StartMoving();
        isAttacking = false;
    }
}
