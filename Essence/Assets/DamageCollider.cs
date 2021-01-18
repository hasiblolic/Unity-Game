using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        if (damagable != null)
            damagable.OnDamage();
    }
}
