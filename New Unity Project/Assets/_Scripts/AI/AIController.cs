using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour, ILockable
{
    new Rigidbody rigidbody;
    Animator anim;
    AnimatorHook animHook;
    NavMeshAgent agent;
    Transform mTransform;
    Controller currentTarget;

    public float fovRadius = 20;
    public float attackDistance = 5;
    public float rotationSpeed = 1;
    public float moveSpeed = 1;
    public bool isInteracting;
    LayerMask detectionLayer;

    private void Start()
    {
        detectionLayer = (1 << 8);
        mTransform = this.transform;
        rigidbody = GetComponentInChildren<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponentInChildren<NavMeshAgent>();
        agent.enabled = false;
        rigidbody.isKinematic = false;
        animHook = GetComponentInChildren<AnimatorHook>();
    }

    private void Update()
    {
        float delta = Time.deltaTime;
        isInteracting = anim.GetBool("isInteracting");

        if (currentTarget == null)
        {
            HandleDetection();
        }
        else
        {
            if (!isInteracting || animHook.canRotate)
            {
                HandleRotation(delta);
                Vector3 lookPosition = currentTarget.mTransform.position;
                lookPosition.y += 1.2f;
                animHook.lookAtPosition = lookPosition;


                if (!isInteracting)
                {
                    anim.SetFloat("movement", 1, 0.1f, delta);

                    float dis = Vector3.Distance(mTransform.position, currentTarget.mTransform.position);
                    if (dis < attackDistance)
                    {
                        PlayTargetAnimation("Attack 1", true);
                    }
                }
            }

            Vector3 targetVelocity = animHook.deltaPosition * moveSpeed;
            rigidbody.velocity = targetVelocity;
        }
    
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }

    void HandleRotation(float delta)
    {
        Vector3 dir = currentTarget.mTransform.position - mTransform.position;
        dir.y = 0;
        dir.Normalize();

        if (dir == Vector3.zero)
        {
            dir = mTransform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRotation, delta / rotationSpeed);
    }


    void HandleDetection()
    {
        Collider[] cols = Physics.OverlapSphere(mTransform.position, fovRadius, detectionLayer);

        for (int i = 0; i < cols.Length; i++)
        {
            Controller controller = cols[i].transform.GetComponentInParent<Controller>();
            if(controller != null)
            {
                // now have aggro
                currentTarget = controller;
                animHook.hasLookAtTarget = true;
                animHook.lookAtPosition = controller.mTransform.position;
                return;
            }
        }
    }

    public Transform GetLockOnTarget(Transform from)
    {
        return mTransform;
    }
}
