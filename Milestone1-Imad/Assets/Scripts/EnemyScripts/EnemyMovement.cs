using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    private EnemyStatsHolder statsHolder;
    private NavMeshAgent agent;
    private Animator anim;
    bool isAggro = false;
    private float lastSeenTime = -Mathf.Infinity;
    public bool isMoving = true;


    void Awake()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = statsHolder.moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis   = true;
        target = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null || !isMoving || statsHolder.isDead) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= statsHolder.detectionRange)
        {
            isAggro = true;
            lastSeenTime = Time.time;
        }

        if (isAggro && Time.time - lastSeenTime > statsHolder.perseveranceTime)
        {
            isAggro = false;
        }

        if (!isAggro)
        {
            agent.isStopped = true;
            anim.SetFloat("MoveSpeed", 0f);
            return;
        }
        if (dist > statsHolder.attackRange)
        {
            agent.isStopped = false;
            agent.speed = statsHolder.moveSpeed;
            agent.SetDestination(target.position);

            Vector3 vel = agent.desiredVelocity;
            if (vel.sqrMagnitude > 0.01f)
                RotateTowards(vel.normalized);
        }
        else
        {
            agent.isStopped = true;
            Vector3 dirToPlayer = (target.position - transform.position).normalized;
            if (dirToPlayer.sqrMagnitude > 0.01f)
                RotateTowards(dirToPlayer);
        }

        float speedPct = agent.velocity.magnitude / statsHolder.moveSpeed;
        anim.SetFloat("MoveSpeed", speedPct);
    }

    public void StopMoving()
    {
        isMoving = false;
        agent.isStopped = true;
        anim.SetFloat("MoveSpeed", 0f);
    }
    public void StartMoving()
    {
        isMoving = true;
        agent.isStopped = false;
    }

    private void RotateTowards(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            statsHolder.rotationSpeed * Time.deltaTime
        );
    }
}
