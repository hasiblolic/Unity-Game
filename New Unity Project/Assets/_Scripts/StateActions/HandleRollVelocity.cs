using UnityEngine;
using System.Collections;

public class HandleRollVelocity : StateAction
{
    PlayerStateManager states;

    bool isInit;
    float t;
    float speed;

    public HandleRollVelocity(PlayerStateManager psm)
    {
        this.states = psm;
        speed = 1 / states.rollClip.length;
    }

    public override bool Execute()
    {
        if (!states.isRolling)
        {
            isInit = false;
        }

        if (!isInit)
        {
            t = 0;
            states.isRolling = true;
            isInit = true;
        }
        float frontY = 0;
        RaycastHit hit;
        Vector3 targetVelocity = states.rollDirection;

        Vector3 origin = states.mTransform.position + (states.mTransform.forward * states.frontRayOffset); // direction you want to go
        origin.y += .5f;

        if (Physics.Raycast(origin, -Vector3.up, out hit, 1, states.ignoreForGroundCheck))
        {
            float y = hit.point.y;
            frontY = y - states.mTransform.position.y;
        }
        states.rigidbody.isKinematic = false;
        states.rigidbody.drag = 0;

        Vector3 currentVelocity = states.rigidbody.velocity;

        if (states.isGrounded)
        {
            if (Mathf.Abs(frontY) > 0.02f)
            {
                targetVelocity.y = ((frontY > 0) ? frontY + 0.2f : frontY - 0.2f) * states.rollSpeed;
            }
        }
        else
        {
            targetVelocity.y = currentVelocity.y;
        }

        if (states.isRolling)
        {
            t += speed * states.delta;
            if (t > 1)
            {
                t = 1;
                states.isRolling = false;
            }
        }
        
        float evaluation = states.rollCurve.Evaluate(t);

        states.rigidbody.velocity = targetVelocity * states.rollSpeed * evaluation;
        return false;
    }
}
