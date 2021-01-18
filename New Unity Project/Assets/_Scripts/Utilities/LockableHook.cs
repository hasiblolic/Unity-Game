using UnityEngine;
using System.Collections;

public class LockableHook : MonoBehaviour, ILockable
{
    public Transform GetLockOnTarget(Transform from)
    {
        return transform;
    }
}
