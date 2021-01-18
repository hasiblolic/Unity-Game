using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerEquipment
{
    // Player's equipment
    public List<Equipment> equipment;
    public bool isDualWielding {
        get {
            if(equipment[(int)EquipmentSlot.LeftWeapon] == null || equipment[(int)EquipmentSlot.RightWeapon] == null)
                return false;
            else return true;
        }
    }

    public void EquipItem(Equipment item) {
        if(item == null) return;

    }
}


public enum EquipmentSlot {
    Head,
    Shoulders,
    Chest,
    Legs,
    Feet,
    Hands,
    LeftWeapon,
    RightWeapon
}