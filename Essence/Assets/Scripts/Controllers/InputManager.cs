using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CameraManager camera;
    public Controller controller;
    public Transform cameraTransform;

    public bool rb, rt, lb, lt, isAttacking, a, b, x, y;
    public bool lockOn;

    public float vertical, horizontal, camX, camY;
    
    float moveAmount;
    float rollFlag;
    float rollTimer;
    Vector2 moveDirection;
    Vector2 cameraDirection;
    PlayerControls playerControls;

    public PlayerProfile playerProfile;


    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Movement.performed += i => moveDirection = i.ReadValue<Vector2>();
        playerControls.Player.Camera.performed += i => cameraDirection = i.ReadValue<Vector2>();
        playerControls.Player.LockOn.started += i => lockOn = true;
        playerControls.Enable();

        cameraTransform = Camera.main.transform;
        ResourcesManager rm = GameSettings.resourcesManager;

        controller.Init();
        //controller.SetWeapons();
        camera.targetTransform = controller.transform;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        float delta = Time.fixedDeltaTime;
        HandleInput();
        camera.HandleRotation(delta, camX, camY);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        HandleMovement(delta);
        camera.FollowTarget(delta);
    }

    bool GetButtonStatus(UnityEngine.InputSystem.InputActionPhase phase)
    {
        return phase == UnityEngine.InputSystem.InputActionPhase.Started;
    }

    private void HandleInput()
    {
        bool retVal = false;
        isAttacking = false;

        vertical = moveDirection.y;
        horizontal = moveDirection.x;
        camX = cameraDirection.x;
        camY = cameraDirection.y;

        rb = GetButtonStatus(playerControls.Player.RB.phase);
        rt = GetButtonStatus(playerControls.Player.RT.phase);
        lb = GetButtonStatus(playerControls.Player.LB.phase);
        lt = GetButtonStatus(playerControls.Player.LT.phase);

        a = GetButtonStatus(playerControls.Player.A.phase);
        b = GetButtonStatus(playerControls.Player.B.phase);
        y = GetButtonStatus(playerControls.Player.Y.phase);
        x = GetButtonStatus(playerControls.Player.X.phase);

        moveAmount = Mathf.Clamp01(Mathf.Abs(vertical) + Mathf.Abs(horizontal));
        if (controller.isInteracting == false)
        {
            if (retVal == false)
                retVal = HandleRolls();
        }

        if (retVal == false)
            retVal = HandleAttacking();

        // locking onto a target
        if (lockOn)
        {
            lockOn = false;
            if (controller.isLockedOn)
            {
                camera.lockTarget = null;
                controller.isLockedOn = false;
                controller.currentTarget = null;

            }
            else
            {
                Transform lockTarget = controller.FindLockableTarget();

                if (lockTarget != null)
                {
                    camera.lockTarget = lockTarget;
                    controller.isLockedOn = true;
                    controller.currentTarget = lockTarget;
                }
                else
                {
                    camera.lockTarget = null;
                    controller.isLockedOn = false;
                }
            }
        }
    }

    private void HandleMovement(float delta)
    {
        Vector3 moveDirection = cameraTransform.right * horizontal;
        moveDirection += cameraTransform.forward * vertical;
        moveDirection.Normalize();

        controller.MoveCharacter(vertical, horizontal, moveDirection, delta);
    }

    private bool HandleRolls()
    {
        if (b == true)
        {
            if (moveAmount > 0)
            {
                // roll in direction you are facing
                Vector3 movementDirection = cameraTransform.right * horizontal;
                movementDirection += cameraTransform.forward * vertical;
                movementDirection.Normalize();
                movementDirection.y = 0;

                Quaternion dir = Quaternion.LookRotation(movementDirection);
                controller.transform.rotation = dir;

                // play roll animation
                controller.PlayTargetAnimation("Roll", true, false);
                return true;
            }
            else
            {
                // if controller is staying still, the step back animation will play
                controller.PlayTargetAnimation("Step Back", true, false);
            }
        }

        return false;
    }

    private bool HandleAttacking()
    {
        AttackInputs attackInput = AttackInputs.none;

        if (rb || rt || lb || lt)
        {
            isAttacking = true;

            if (rb)
            {
                attackInput = AttackInputs.rb;
            }

            if (rt)
            {
                attackInput = AttackInputs.rt;
            }
            if (lb)
            {
                attackInput = AttackInputs.lb;
            }
            if (lt)
            {
                attackInput = AttackInputs.lt;
            }
        }
        return isAttacking;
    }

}
