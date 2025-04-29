using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float stoppingDistance = 1.5f;
    private EnemyStatsHolder statsHolder;
    private NavMeshAgent agent;
    private Animator anim;
    bool isAggro = false;
    private float lastSeenTime = -Mathf.Infinity;
    bool isMoving = true;

    void Awake()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = statsHolder.moveSpeed;
        agent.updateRotation = true;
        agent.updateUpAxis   = true;
    }

    void Update()
    {
        if (target == null || !isMoving) return;

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
            anim.SetFloat("MoveSpeed", 0f);
        }
        else
        {
            if (dist <= stoppingDistance)
                agent.isStopped = true;
            else
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }

        float speedPct = agent.velocity.magnitude / statsHolder.moveSpeed;
        anim.SetFloat("MoveSpeed", speedPct);
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }
}
