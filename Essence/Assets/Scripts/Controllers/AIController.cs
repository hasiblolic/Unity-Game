using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour, ILockable, IDamagable
{
    public Controller target;

    [Header("Character Values")]
    public CharacterAttributes stats;
    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;


    [Header("Movement Values")]
    public float moveSpeed = 2;
    public float rotateSpeed = 1;
    private Transform myTransform;

    [Header("AI Values")]
    public float fovRadius = 20;
    public float targetRange = 3;
    private Vector3 homePosition;

    [Header("AI Action Values")]
    public int hardcodeAction = -1; // set to a value to play ONLY selected animation
    public ActionSnapshot[] actions;
    public ActionSnapshot currentAction;
    public bool actionFlag;
    public float recoveryTimer;

    [Header("States")]
    public bool isInteracting;
    public float hitTimer;
    public bool isHit;
    
    private Animator animator;
    private AnimatorHook animatorHook;
    private NavMeshAgent agent;
    private new Rigidbody rigidbody;
    public WeaponManager weaponManager;
    public ClothManager clothManager;
    private LayerMask detectionLayer;

    // Start is called before the first frame update
    void Start()
    {
        detectionLayer = (1 << 8);
        animator = GetComponentInChildren<Animator>();
        animatorHook = GetComponentInChildren<AnimatorHook>();
        agent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        weaponManager = GetComponent<WeaponManager>();
        clothManager = GetComponent<ClothManager>();

        rigidbody.isKinematic = false;
        myTransform = this.transform;
        homePosition = this.transform.position;

        agent.stoppingDistance = 1;

        weaponManager.Init();
        weaponManager.LoadWeaponOnHook(rightWeapon, false);
        weaponManager.LoadWeaponOnHook(leftWeapon, true);
    }

    private void LateUpdate()
    {
        agent.transform.localPosition = Vector3.zero;
        agent.transform.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = animator.GetBool("isInteracting");

        // getting hit
        if (isHit)
        {
            if (hitTimer > 0)
                hitTimer -= delta;
            else isHit = false;
        }


        // detection
        if(target == null)
        {
            HandleDetection();
        }
        else
        {
            Vector3 dir = target.currentTransform.position - myTransform.position;
            dir.y = 0;
            dir.Normalize();
            float distance = Vector3.Distance(myTransform.position, target.transform.position);
            float angle = Vector3.Angle(myTransform.forward, dir);
            float dot = Vector3.Dot(myTransform.right, dir);
            if (dot < 0)
                angle *= -1;

            if (agent.isActiveAndEnabled && distance > targetRange)
                agent.SetDestination(target.currentTransform.position);
            else HandleRotation(delta);

            Vector3 relativeDirection = myTransform.InverseTransformDirection(agent.desiredVelocity);

            if(!isInteracting)
            {
                if (actionFlag)
                {
                    recoveryTimer -= delta;
                    if (recoveryTimer <= 0)
                        actionFlag = false;
                }

                animator.SetFloat("Vertical", relativeDirection.z, 0.1f, delta);

                currentAction = GetAction(distance, angle);
                if(currentAction != null && !actionFlag)
                {
                    PlayTargetAnimation(currentAction.targetAnimationName, true);
                    actionFlag = true;
                    recoveryTimer = currentAction.recoveryTime;
                }
                else
                {
                    animator.SetFloat("Horizontal", relativeDirection.x, 0.1f, delta);
                }

                agent.enabled = true;
                myTransform.rotation = agent.transform.rotation;
                Vector3 lookPosition = target.currentTransform.position;
                lookPosition.y += 1.2f;
                animatorHook.lookPosition = lookPosition;
            }
            else
            {
                if (animatorHook.canRotate)
                    HandleRotation(delta);
                agent.enabled = false;
                animator.SetFloat("Vertical", 0, 0.1f, delta);
                animator.SetFloat("Horizontal", 0, 0.1f, delta);
            }

            Vector3 targetVelocity = animatorHook.deltaPosition * moveSpeed;
            rigidbody.velocity = targetVelocity;
        }
    }


    #region Movement

    public void HandleRotation(float delta)
    {
        Vector3 dir = target.currentTransform.position - myTransform.position;
        dir.y = 0;
        dir.Normalize();

        if (dir == Vector3.zero)
        {
            dir = myTransform.forward;
        }

        float angle = Vector3.Angle(dir, myTransform.forward);
        if(angle > 5)
        {
            animator.SetFloat("Horizontal", Vector3.Dot(dir, myTransform.right), 0.1f, delta);
        }
        else
        {
            animator.SetFloat("Horizontal", 0, 0.1f, delta);
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRot, delta / rotateSpeed);
    }

    #endregion

    #region Actions

    public ActionSnapshot GetAction(float distance, float angle)
    {
        if(hardcodeAction != -1)
        {
            int index = hardcodeAction;
            return actions[index];
        }

        int maxWeight = 0;
        for (int i = 0; i < actions.Length; i++)
        {
            ActionSnapshot a = actions[i];
            if(distance <= a.attackDistance)
            {
                if(angle <= a.maxAngle && angle >= a.minAngle)
                {
                    maxWeight += a.weight;
                }
            }
        }

        int rand = Random.Range(0, maxWeight + 1);
        int temp = 0;

        for (int i = 0; i < actions.Length; i++)
        {
            ActionSnapshot a = actions[i];
            if (a.weight == 0)
                continue;

            if (distance <= a.attackDistance)
            {
                if(angle <= a.maxAngle && angle >= a.minAngle)
                {
                    temp += a.weight;
                    if (temp > rand)
                        return a;
                }
            }
        }

        return null;
    }

    public void HandleAttacking()
    {

    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void HandleDetection()
    {
        Collider[] cols = Physics.OverlapSphere(myTransform.position, fovRadius, detectionLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            Controller controller = cols[i].transform.GetComponentInParent<Controller>();
            if(controller != null)
            {
                target = controller;
                animatorHook.hasTarget = true;
                return;
            }
        }
    }

    #endregion

    #region Interface Related

    public void OnDamage()
    {
        if (!isHit)
        {
            isHit = true;
            hitTimer = 2;

            stats.health.CurrentValue -= 200;

            if(stats.health.CurrentValue > 0)
            {
                PlayTargetAnimation("Damage 1", true);
            }
            else
            {
                PlayTargetAnimation("Death", true);
                animator.transform.parent = null;
                gameObject.SetActive(false);
            }
        }
    }

    public Transform GetLockOnTarget(Transform from)
    {
        if (stats.health.CurrentValue > 0)
            return myTransform;
        else return null;
    }

    #endregion
}

// used to select an action based on weight of action (percent chance), distance, angle, etc.
[System.Serializable]
public class ActionSnapshot
{
    public string targetAnimationName;
    public int weight;
    public float recoveryTime;
    public float attackDistance;
    public float minAngle;
    public float maxAngle;
}
