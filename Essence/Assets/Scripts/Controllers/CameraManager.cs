using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform targetTransform;
    public Transform lockTarget;
    public Transform pivot;
    public float followSpeed = 0.1f;
    public float resetSpeed = 3;

    [Header("Adjustable Values")]
    public float lookSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    public float velSpeed = .1f;
    public float minPivot = -35;
    public float maxPivot = 35;

    private Transform myTransform;
    private float lookAngle;
    private float pivotAngle;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.transform;
    }

    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
        myTransform.position = targetPosition;
    }

    public void HandleRotation(float delta, float mouseX, float mouseY)
    {
        if(lockTarget == null)
        {
            lookAngle += (mouseX * lookSpeed) / delta;
            pivotAngle -= (mouseY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 euler = Vector3.zero;
            euler.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(euler);
            myTransform.rotation = targetRotation;

            euler = Vector3.zero;
            euler.x = pivotAngle;
            targetRotation = Quaternion.Euler(euler);
            pivot.localRotation = targetRotation;

            Quaternion resetRotation = Quaternion.Slerp(myTransform.rotation, targetTransform.rotation, delta / resetSpeed);
            lookAngle = resetRotation.eulerAngles.y;
        }
        else
        {
            Vector3 dir = lockTarget.position - myTransform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            myTransform.rotation = targetRotation;

            dir = lockTarget.position - pivot.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 e = targetRotation.eulerAngles;
            e.y = 0;
            pivot.localEulerAngles = e;

            pivotAngle = pivot.localEulerAngles.x;
            lookAngle = myTransform.eulerAngles.y;
        }
    }
}
