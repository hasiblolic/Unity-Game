using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class should be attached to the model that has the animator script attached / controller should be in parent object
public class AnimatorHook : MonoBehaviour
{
    Animator animator;
    Controller controller;
    AIController aiController;

    [Header("AI")]
    public bool isAI;
    public bool hasTarget;
    public Vector3 lookPosition;

    [Header("States")]
    public bool canRotate;
    public bool canDoCombo;
    public Vector3 deltaPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<Controller>();

        if (controller == null)
        {
            isAI = true;
            aiController = GetComponentInParent<AIController>();
        }
        else isAI = false;
    }

    public void OnAnimatorMove()
    {
        OnAnimatorMoveOverride();
    }

    protected virtual void OnAnimatorMoveOverride()
    {
        float delta = Time.deltaTime;
        if (!isAI)
        {
            if (controller.isInteracting == false)
                return;

            if (controller.isGrounded && delta > 0)
                deltaPosition = (animator.deltaPosition) / delta;
        }
        else
        {
            deltaPosition = (animator.deltaPosition) / delta;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (hasTarget)
        {
            animator.SetLookAtWeight(1, 0.9f, 0.95f, 1, 1);
            animator.SetLookAtPosition(lookPosition);
        }
    }

    public void OpenCanMove()
    {
        if (!isAI)
            controller.canMove = true;
    }

    public void OpenDamageCollider()
    {
        if (!isAI)
        {
            // first find if there is a weapon equipped
            if (controller.leftEquippedWeapon == null && controller.rightEquippedWeapon == null)
                return; // no weapon equipped so there is no collider

            // find if it is a left hook or not
            bool isLeftHook = false;
            if (controller.leftEquippedWeapon == null)
                isLeftHook = false; // no weapon equipped
            else isLeftHook = true;

            controller.weaponManager.SetDamageCollider(isLeftHook, true);
        }
        else
        {
            // first find if there is a weapon equipped
            if (aiController.leftWeapon == null && aiController.rightWeapon == null)
                return; // no weapon equipped so there is no collider

            // find if it is a left hook or not
            bool isLeftHook = false;
            if (aiController.leftWeapon == null)
                isLeftHook = false; // no weapon equipped
            else isLeftHook = true;

            aiController.weaponManager.SetDamageCollider(isLeftHook, true);
        }
    }

    public void CloseDamageCollider()
    {
        if (!isAI)
        {
            // first find if there is a weapon equipped
            if (controller.leftEquippedWeapon == null && controller.rightEquippedWeapon == null)
                return; // no weapon equipped so there is no collider

            // find if it is a left hook or not
            bool isLeftHook = false;
            if (controller.leftEquippedWeapon == null)
                isLeftHook = false; // no weapon equipped
            else isLeftHook = true;

            controller.weaponManager.SetDamageCollider(isLeftHook, false);
        }
        else
        {
            // first find if there is a weapon equipped
            if (aiController.leftWeapon == null && aiController.rightWeapon == null)
                return; // no weapon equipped so there is no collider

            // find if it is a left hook or not
            bool isLeftHook = false;
            if (aiController.leftWeapon == null)
                isLeftHook = false; // no weapon equipped
            else isLeftHook = true;

            aiController.weaponManager.SetDamageCollider(isLeftHook, false);
        }
    }

    public void EnableCombo()
    {
        canDoCombo = true;
    }

    public void DisableCombo()
    {
        canDoCombo = false;
    }

    public void EnableRotation()
    {
        canRotate = true;
    }

    public void DisableRotation()
    {
        canRotate = false;
    }
}
