using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public PlayerInventory inventory;
    public PlayerController playerController; 

    public bool inputLocked = false;
    private int currentComboIndex = 0;
    private bool canCombo = false;
    private Coroutine activeAttackCoroutine;

    void Update()
    {
        ItemData item = inventory.CurrentItem;
        if (item == null || item.comboSequence.Count == 0) return;

        if (Input.GetKey(KeyCode.JoystickButton2) && (!inputLocked || canCombo) )
        {
            if (activeAttackCoroutine != null)
                StopCoroutine(activeAttackCoroutine);
            print("pressed attack button");
            AttackStep step = item.comboSequence[currentComboIndex];
            activeAttackCoroutine = StartCoroutine(PerformAttack(step));
        }
    }

    private IEnumerator PerformAttack(AttackStep step)
    {
        canCombo = false;
        inputLocked = true;

        if (playerController != null)
        {
            playerController.canMove = false;

            Rigidbody rb = playerController.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }
        }

        if (!string.IsNullOrEmpty(step.animationTrigger))
            animator.SetTrigger(step.animationTrigger);

        
        animator.SetBool("CanMoveInterrupt", false);
        animator.SetBool("CanReturnToBlendTree", false);
        inventory.canNavigate = false;

        yield return new WaitForSeconds(step.lockoutDuration);
        animator.SetBool("CanMoveInterrupt", true);

        if (step.comboWindowStart > 0)
        {
            yield return new WaitForSeconds(step.comboWindowStart - step.lockoutDuration);
            print("combo window start waited : " + (step.comboWindowStart - step.lockoutDuration).ToString());
            canCombo = true;
            currentComboIndex += 1;
            yield return new WaitForSeconds(step.comboWindowEnd - step.comboWindowStart);
            print("combo window END waited : " + (step.comboWindowEnd - step.comboWindowStart).ToString());
            currentComboIndex = 0;
        }
    }

    public void EndAttackMovementLock()
    {
        animator.SetBool("CanReturnToBlendTree", true);
        playerController.canMove = true;
        animator.SetBool("CanMoveInterrupt", false);
        inputLocked = false;
        inventory.canNavigate = true;
        currentComboIndex = 0;
    }


}
