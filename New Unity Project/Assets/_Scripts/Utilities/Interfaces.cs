using UnityEngine;
using System.Collections;

public interface ILockable
{
    Transform GetLockOnTarget(Transform from);
}
