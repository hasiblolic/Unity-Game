using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    WeaponItem leftItem;
    WeaponItem rightItem;

    WeaponHook leftHook;
    WeaponHook rightHook;

    public void Init()
    {
        WeaponHook[] weaponHooks = GetComponentsInChildren<WeaponHook>();
        foreach(WeaponHook hook in weaponHooks)
        {
            if (hook.isLeftHand)
                leftHook = hook;
            else 
                rightHook = hook;
        }
    }

    public void LoadWeaponOnHook(WeaponItem item, bool isLeft)
    {
        if (isLeft)
        {
            leftHook.LoadWeaponModel(item);
            leftItem = item;
        }
        else
        {
            rightHook.LoadWeaponModel(item);
            rightItem = item;
        }
    }

    public void SetDamageCollider(bool isLeftHook, bool status)
    {
        if (isLeftHook)
        {
            leftHook.SetDamageCollider(status);
        }
        else
        {
            rightHook.SetDamageCollider(status);
        }
    }
}
