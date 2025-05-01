using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base Stats")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float dashForce = 10f;
    public float dashCooldown = 0.5f;
    public Animator animator;

    [Header("Costs")]
    public float dashCost = 10f;
    public float staminaCostPerSecondWhileRunning = 2f;

    private Rigidbody rb;
    private Vector3 inputDir;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isRunning = false;
    private PlayerStats stats;
    private bool isAlive = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!isAlive) return; 

        float moveX = -Input.GetAxisRaw("Horizontal");
        float moveZ = -Input.GetAxisRaw("Vertical");

        Vector3 rawInput = new Vector3(moveX, 0f, moveZ);
        bool isMoving = rawInput.sqrMagnitude > 0f;

        if (!isDashing)
        {
            if (isMoving)
            {
                inputDir = rawInput.normalized;
                transform.rotation = Quaternion.LookRotation(inputDir, Vector3.up);
            }
            else
            {
                inputDir = Vector3.zero;
            }
            bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Sprint") > 0;
            isRunning = shiftHeld && isMoving && stats.currentStamina > 0f;

            if (isRunning)
            {
                float staminaCostThisFrame = staminaCostPerSecondWhileRunning * Time.deltaTime;
                if (!stats.SpendStamina(staminaCostThisFrame))
                {
                    isRunning = false;
                }
            }

            if (animator != null)
            {
                animator.SetFloat("Speed", rb.velocity.magnitude);
            }

            if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.JoystickButton0)) && canDash)
            {
                Dash();
            }

            // stats.TakeDamage(20f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (!isAlive) return; 

        if (!isDashing)
        {
            float targetSpeed = isRunning ? runSpeed : walkSpeed;
            rb.velocity = inputDir * targetSpeed;
        }
    }

    void Dash()
    {
        if (!isAlive) return; 

        if (!stats.SpendStamina(dashCost))
            return;

        canDash = false;
        isDashing = true;

        if (animator != null)
        {
            animator.SetTrigger("Dash");
        }

        Vector3 dashDir = inputDir != Vector3.zero ? inputDir : transform.forward;
        rb.velocity = Vector3.zero;
        rb.AddForce(dashDir * dashForce, ForceMode.VelocityChange);

        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown - 0.2f);
        canDash = true;
    }

    public void Die()
    {
        isAlive = false; 
        rb.velocity = Vector3.zero;
        if (animator != null)
        {
            animator.SetTrigger("Die"); 
        }
    }

    public void Revive()
    {
        isAlive = true; 
    }
}
