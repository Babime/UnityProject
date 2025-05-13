using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsHolder : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float perseveranceRange = 2f;
    public float detectionRange = 5f;
    public float rotationSpeed = 720f;


    [Header("Combat")]
    public float attackRange = 3f;
    public float attackCooldown = 1.2f;
    public int damage = 10;
    public float attackAngle = 30f;

    [Header("Combat (Distance)")]
    public float rangedAttackCooldown = 2f;
    public float fireDelay = 0.4f;
    public float projectileSpeed = 12f;
    public int projectileDamage = 20;

    [Header("Spawn")]
    public float spawnDuration = 4f;

    [HideInInspector]
    public bool isDead = false;

}
