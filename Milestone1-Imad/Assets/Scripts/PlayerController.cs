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
    private bool isUsingItem = false;
    private PlayerStats stats;
    PlayerCombat combat;
    public PlayerInventory inventory;
    private bool isAlive = true;

    [Header("Control Flags")]
    public bool canMove = true;
    private GameObject goldAura;
    private GameObject healingAura;
    public GameObject bloodAura;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        stats = GetComponent<PlayerStats>();
        combat = GetComponent<PlayerCombat>();
        goldAura = GameObject.Find("Player Gold Aura");
        healingAura = GameObject.Find("Player Healing Aura");
        bloodAura = GameObject.Find("Player Blood Aura");
        if (goldAura != null) goldAura.SetActive(false);
        if (healingAura != null) healingAura.SetActive(false);
        if (bloodAura != null) bloodAura.SetActive(false);
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
            if (canMove && isMoving)
            {
                inputDir = rawInput.normalized;
                transform.rotation = Quaternion.LookRotation(inputDir, Vector3.up);
            }
            else if (!canMove || !isMoving)
            {
                inputDir = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton2) && !isUsingItem)
            {
                if (inventory.CurrentItem != null && 
                    (inventory.CurrentItem.hpRegenAmount > 0 || inventory.CurrentItem.staminaRegenAmount > 0))
                {
                    
                    canMove = false;
                    canDash = false;
                    isUsingItem = true;
                    inventory.canNavigate = false;
                    inputDir = Vector3.zero;
                    rb.velocity = Vector3.zero;
                    animator.SetBool("CanReturnToBlendTree", false);

                    if (inventory.CurrentItem.auraPrefab == "Player Gold Aura")
                    {
                        goldAura.SetActive(true);
                    }
                    else if (inventory.CurrentItem.auraPrefab == "Player Healing Aura")
                    {
                        healingAura.SetActive(true);
                    }

                    if (animator != null)
                    {
                        animator.SetTrigger("Interact");
                    }

                    if (inventory.CurrentItem.hpRegenAmount > 0)
                    {
                        stats.Heal(inventory.CurrentItem.hpRegenAmount);
                    }

                    if (inventory.CurrentItem.staminaRegenAmount > 0)
                    {
                        stats.RecoverStamina(inventory.CurrentItem.staminaRegenAmount);
                    }

                    StartCoroutine(ItemInUseCooldown());
                }
            }

            bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Sprint") > 0;
            isRunning = canMove && shiftHeld && isMoving && stats.currentStamina > 0f;

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

            
        }
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        if (!isDashing && canMove)
        {
            float targetSpeed = isRunning ? runSpeed : walkSpeed;
            rb.velocity = inputDir * targetSpeed;
        }
    }

    void Dash()
    {
        if (!isAlive) return;

        if ((combat.inputLocked && animator.GetBool("CanMoveInterrupt") == false) || !stats.SpendStamina(dashCost) )
            return;
        
        canDash = false;
        isDashing = true;

        if (animator != null)
        {
            animator.SetTrigger("Dash");
        }

        float moveX = -Input.GetAxisRaw("Horizontal");
        float moveZ = -Input.GetAxisRaw("Vertical");
        Vector3 dashInput = new Vector3(moveX, 0f, moveZ).normalized;

        Vector3 dashDir = dashInput.sqrMagnitude > 0f ? dashInput : inputDir;
        
        if (!canMove) {
            transform.rotation = Quaternion.LookRotation(dashDir, Vector3.up);
            combat.EndAttackMovementLock();
        }

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

    IEnumerator ItemInUseCooldown()
    {
        yield return new WaitForSeconds(1.2f);
        
        if (inventory.CurrentItem.auraPrefab == "Player Gold Aura")
        {
            goldAura.SetActive(false);
        }
        else if (inventory.CurrentItem.auraPrefab == "Player Healing Aura")
        {
            healingAura.SetActive(false);
        }

        animator.SetBool("CanReturnToBlendTree", true);
        inventory.RemoveCurrentItemFromInventory();
        canMove = true;
        canDash = true;
        isUsingItem = false;
        inventory.canNavigate = true;
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



