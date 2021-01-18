using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Item> inventory;

    public void AddItem(Item item) {
        if(item == null) return;
        
        // adding new item to inventory, check if you already have one and just add to stack
        if(inventory.Contains(item))
            inventory.Find(x => x.itemID == item.itemID).itemAmount += item.itemAmount;
        else inventory.Add(item); // if it isn't already in the inventory, add it as a new item
    }

    public void RemoveItem(Item item) {
        if(item == null) return;

        inventory.Remove(item);
    }
}
