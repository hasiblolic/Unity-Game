using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Item Values")]
    public string itemName;
    public string itemDescription;
    public string itemID;
    public int itemValue;
    public int itemWeight;
}
