using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsHolder : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float perseveranceTime = 2f;
    public float detectionRange = 5f;

    [Header("Combat")]
    public float attackRange = 3f;
    public float attackCooldown = 1.2f;
    public int damage = 10;
    public float attackAngle = 30f;

    [Header("Spawn")]
    public float spawnDuration = 4f;

}
