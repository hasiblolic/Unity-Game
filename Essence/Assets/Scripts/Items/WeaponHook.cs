using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// the weaponhook should be placed on each hand of the character and in case it is left handed the bool isLeftHand should be ticked
public class WeaponHook : MonoBehaviour
{
    public Transform parentOverride;
    public GameObject damageCollider;
    public bool isLeftHand;

    GameObject currentModel;

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        if(weaponItem == null)
        {
            // there is no item so deactivate any existing models on the hook
            UnloadWeapon();
            return;
        }

        // instantiating a new object from the item's model
        GameObject model = Instantiate(weaponItem.itemModel) as GameObject;


        // setting position of the model
        if(model != null)
        {
            if(parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = this.transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one * 30;
        }

        currentModel = model;

        if(currentModel != null)
        {
            BoxCollider col = currentModel.GetComponentInChildren<BoxCollider>();
            if (col != null)
                damageCollider = col.gameObject;
            SetDamageCollider(false);
        }
    }

    public void SetDamageCollider(bool status)
    {
        damageCollider.SetActive(status);
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }
    }

    public void UnloadWeapon()
    {
        if(currentModel != null)
        {
            currentModel.SetActive(false);
        }
    }
}
