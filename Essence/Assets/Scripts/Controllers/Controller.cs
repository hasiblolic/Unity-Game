using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(ClothManager))]
public class Controller : MonoBehaviour, IDamagable
{
    // references
    public WeaponManager weaponManager;
    public ClothManager clothManager;
    public UIManager uiManager;
    private Animator animator;
    private AnimatorHook animatorHook;
    private new Rigidbody rigidbody;
    private LayerMask ignoreGroundCheck;

    // positions/movement
    [HideInInspector]
    public Transform currentTransform;
    private Vector3 currentPosition;
    private Vector3 currentNormal;
    [HideInInspector]
    public Transform currentTarget;

    [Header("Movement Values")]
    public float moveSpeed = 3;
    public float groundedSpeed = 0.1f;
    public float attackRotationSpeed = 3;
    public float rotateSpeed = 10f;
    public float velocityMultiplier = 1f;
    public float rollSpeed = 1;
    public AnimationCurve rollCurve;
    public AnimationClip rollClip;

    [Header("States")]
    public bool isInteracting;
    public bool isGrounded;
    public bool isLockedOn;
    public bool canMove;
    public bool isHit;
    public float hitTimer;

    [Header("Falling Related Values")]
    public float groundDownDistanceInAir = 0.5f;
    public float groundedDistanceRay = 0.3f;

    [Header("Character Values")]
    public CharacterAttributes stats;

    [Header("Items")]
    public List<Item> inventory;

    [Header("Equipment")]
    public WeaponItem leftEquippedWeapon;
    public WeaponItem rightEquippedWeapon;

    // Start is called before the first frame update
    public void Init()
    {
        currentTransform = this.transform;
        currentPosition = currentTransform.position;

        weaponManager = GetComponent<WeaponManager>();
        clothManager = GetComponent<ClothManager>();
        animator = GetComponentInChildren<Animator>();
        animatorHook = GetComponentInChildren<AnimatorHook>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        uiManager = GetComponent<UIManager>();

        weaponManager.Init();
        weaponManager.LoadWeaponOnHook(rightEquippedWeapon, false);
        weaponManager.LoadWeaponOnHook(leftEquippedWeapon, true);

        ignoreGroundCheck = ~(1 << 9 | 1 << 10);

        stats.SetDefaultAttributes();
        stats.health.CurrentValue = stats.health.Value;
        Debug.Log("current health: " + stats.health.CurrentValue);
        Debug.Log("max health: " + stats.health.Value);
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        // set the interacting state -- using items, jumping, etc.
        isInteracting = animator.GetBool("isInteracting");

        // makes so you can't be hurt multiple times with the same attack instantly
        DamageControl(delta);

    }

    #region Movement

    public void MoveCharacter(float vertical, float horizontal, Vector3 moveDirection, float delta)
    {

        CheckGround();
        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        //HANDLE ROTATION
        if (!isInteracting || animatorHook.canRotate)
        {
            Vector3 rotationDir = moveDirection;

            if (isLockedOn)
            {
                rotationDir = currentTarget.position - currentTransform.position;
            }

            HandleRotation(moveAmount, rotationDir, delta);
        }

        Vector3 targetVelocity = Vector3.zero;

        if (isLockedOn)
        {
            targetVelocity = currentTransform.forward * vertical * moveSpeed;
            targetVelocity += currentTransform.right * horizontal * moveSpeed;
        }
        else
        {
            targetVelocity = moveDirection * moveSpeed;
        }

        if (isInteracting)
        {
            targetVelocity = animatorHook.deltaPosition * velocityMultiplier;
        }

        //HANDLE MOVEMENT
        if (isGrounded)
        {
            targetVelocity = Vector3.ProjectOnPlane(targetVelocity, currentNormal);
            rigidbody.velocity = targetVelocity;

            Vector3 groundedPosition = currentTransform.position;
            groundedPosition.y = currentPosition.y;
            currentTransform.position = Vector3.Lerp(currentTransform.position, groundedPosition, delta / groundedSpeed);
        }

        HandleAnimations(vertical, horizontal, moveAmount);
    }

    public void HandleRotation(float moveAmount, Vector3 targetDirection, float delta)
    {
        float moveOverride = moveAmount;
        if (isLockedOn)
            moveOverride = 1;

        targetDirection.Normalize();
        targetDirection.y = 0;
        if (targetDirection == Vector3.zero)
            targetDirection = currentTransform.forward;

        float actualRotationSpeed = rotateSpeed;
        if (isInteracting)
            actualRotationSpeed = attackRotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDirection);
        Quaternion targetRotation = Quaternion.Slerp(
            currentTransform.rotation, tr, delta * moveOverride * actualRotationSpeed);

        currentTransform.rotation = targetRotation;
    }

    private void CheckGround()
    {
        RaycastHit hit;
        Vector3 origin = currentTransform.position;
        origin.y += 0.5f;

        float distance = groundedDistanceRay;
        if (!isGrounded)
        {
            distance = groundDownDistanceInAir;
        }

        // debugging ray to see the checkground ray emitting from controller
        Debug.DrawRay(origin, Vector3.down * distance, Color.red);
        if(Physics.SphereCast(origin, .2f, Vector3.down, out hit, distance, ignoreGroundCheck))
        {
            // spherecast detected the floor, controller is grounded
            isGrounded = true;
            currentPosition = hit.point;
            currentNormal = hit.normal;
            float angle = Vector3.Angle(Vector3.up, currentNormal);

            // if the floor angle is too steep, we should be falling
            if (angle > 45)
                isGrounded = false;

            // if falling and ground is detected play empty animation
            if (isGrounded == false)
            {
                isGrounded = true;
                PlayTargetAnimation("Empty", false, false);
            }
        }
        else
        {
            // spherecast could not detect the floor, so the controller is in the air
            isGrounded = false;
            PlayTargetAnimation("Falling", true, false);
        }
    }

    #endregion

    #region Actions

    private void HandleAnimations(float v, float h, float moveAmount)
    {
        float vertical = ClampValues(v);
        float horizontal = ClampValues(h);
        
        if (isGrounded)
        {
            if (isLockedOn)
            {
                float lockedHorizontal = horizontal;
                if (lockedHorizontal != 0)
                {
                    if (lockedHorizontal > 0)
                        lockedHorizontal = 0.5f;
                    else lockedHorizontal = -0.5f;
                }

                animator.SetFloat("Vertical", vertical);
                animator.SetFloat("Horizontal", lockedHorizontal);
            }
            else
            {
                float freeVertical = vertical;
                if (freeVertical < 0)
                    freeVertical = 1;
                // in free look camera, you really don't want any strafing going on
                animator.SetFloat("Vertical", freeVertical, 0.02f, Time.deltaTime);
                animator.SetFloat("Horizontal", 0);
            }
        }
        else
        {
            // controller is in air
        }
    }

    // this will return a clamped value of either 0, 0.5, 1, or -x
    private float ClampValues(float clampValue)
    {
        float retVal = 0;
        if (clampValue > 0 && clampValue < 0.5f)
            retVal = 0.5f;
        else if (clampValue > 0.5f)
            retVal = 1;
        else if (clampValue < 0 && clampValue > -0.5f)
            retVal = -0.5f;
        else if (clampValue < -0.5f)
            retVal = -1;

        return retVal;
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool isMirror = false, float velocityMultiplier = 1)
    {
        animator.SetBool("isMirror", isMirror);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
        this.isInteracting = isInteracting;
        this.velocityMultiplier = velocityMultiplier;
    }

    // this will find a target to lock onto with the camera
    public Transform FindLockableTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(currentTransform.position, 20);
        for(int i = 0; i < colliders.Length; i++)
        {
            ILockable lockable = colliders[i].GetComponentInParent<ILockable>();

            // target found so return it
            if (lockable != null)
                return lockable.GetLockOnTarget(currentTransform);
        }

        // nothing was found
        return null;
    }

    #endregion

    #region Interfaces

    public void OnDamage()
    {
        if (!isHit)
        {
            isHit = true;
            hitTimer = 2;
            PlayTargetAnimation("Damage 1", true);
            stats.health.CurrentValue -= 200;
            uiManager.vitals.SetHealthBar((stats.health.CurrentValue / stats.health.Value));
        }
    }

    public void DamageControl(float delta)
    {
        if (isHit)
        {
            if (hitTimer > 0)
                hitTimer -= delta;
            else isHit = false;
        }
    }

    #endregion
}
