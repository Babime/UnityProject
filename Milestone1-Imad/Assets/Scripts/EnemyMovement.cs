using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float stoppingDistance = 1.5f;
    public float rotationSpeed = 720f;

    private EnemyStatsHolder statsHolder;
    private Animator anim;
    bool isAggro = false;
    private float lastSeenTime = -Mathf.Infinity;
    bool isMoving = true;

    void Awake()
    {
        statsHolder = GetComponent<EnemyStatsHolder>();
        anim = GetComponent<Animator>();
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
            if (dist > stoppingDistance)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * statsHolder.moveSpeed * Time.deltaTime;

                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime / 360f
                );

                float speedPct = statsHolder.moveSpeed / statsHolder.moveSpeed; // Always 1
                anim.SetFloat("MoveSpeed", speedPct);
            }
            else
            {
                anim.SetFloat("MoveSpeed", 0f);
            }
        }
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
