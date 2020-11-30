using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : MonoBehaviour
{
    public string playerName;
    public PlayerStats playerStats;
    public Inventory playerInventory;
    public PlayerEquipment playerEquipment;

    public float[] position;
    public int sceneLocation;

    public void SavePlayer() {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer() {
        PlayerData data = SaveSystem.LoadPlayer();
        playerName = data.playerName;

        playerStats = new PlayerStats();
        playerStats = data.playerStats;

        playerInventory = new Inventory();
        playerInventory = data.playerInventory;
        
        playerEquipment = new PlayerEquipment();
        playerEquipment = data.playerEquipment;
        playerEquipment.equipment = new List<Equipment>();
        for(int i = 0; i < System.Enum.GetNames(typeof(EquipmentSlot)).Length; i++) {
            playerEquipment.equipment.Add(new Equipment());
        }

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

        sceneLocation = data.sceneLocation;
    }

    // damageRating = BaseDamage + (BaseDamage*ScalingRating*Saturation)
    // y=(((x-a)^3)/c^2)+b (formula for getting stat scaling -- playerstats.GetStatBonus())
    // goes up rather quickly at lower levels and slows down once you reach around 70
    public int GetPhysicalDamageRating() {
        int leftWeaponIndex = (int)EquipmentSlot.LeftWeapon;
        int rightWeaponIndex = (int) EquipmentSlot.RightWeapon;

        // if no weapon equipped then we will just go off of player's strength stat
        if (playerEquipment.equipment[leftWeaponIndex] == null && playerEquipment.equipment[rightWeaponIndex] == null) return playerStats.strength;
        
        // caching for easier to read
        Equipment left = playerEquipment.equipment[leftWeaponIndex];
        Equipment right = playerEquipment.equipment[rightWeaponIndex];

        // find total of physical damage value
        int strength = 0;
        int dexterity = 0;

        // getting bonus for player's strength
        if(playerEquipment.equipment[leftWeaponIndex] != null)
            strength += (int) (left.basePhysical + (left.basePhysical * left.strengthScaling * playerStats.GetStatBonus(playerStats.strength)));

        if(playerEquipment.equipment[rightWeaponIndex] != null)
            strength += (int) (right.basePhysical + (right.basePhysical * right.strengthScaling * playerStats.GetStatBonus(playerStats.strength)));
        
        // getting bonus for player's dexterity
        if(playerEquipment.equipment[leftWeaponIndex] != null)
            dexterity += (int) (left.basePhysical + (left.basePhysical * left.dexterityScaling * playerStats.GetStatBonus(playerStats.dexterity)));

        if(playerEquipment.equipment[rightWeaponIndex] != null)
            dexterity += (int) (right.basePhysical + (right.basePhysical * right.dexterityScaling * playerStats.GetStatBonus(playerStats.dexterity)));
        
        // if dualwielding reduce the bonus to a reasonable amount
        if(playerEquipment.isDualWielding){
            strength -= (int)(strength / 1.1f);
            dexterity -= (int)(dexterity / 1.1f);
        } 

        return strength + dexterity;
    }

    // this function is pretty much identical to the getphysicaldamagerating but for magic damage
    public int GetMagicalDamageRating() {
        int leftWeaponIndex = (int)EquipmentSlot.LeftWeapon;
        int rightWeaponIndex = (int) EquipmentSlot.RightWeapon;

        // if no weapons are equipped we will just go off of player's wisdom stat
        if (playerEquipment.equipment[leftWeaponIndex] == null && playerEquipment.equipment[rightWeaponIndex] == null) return playerStats.wisdom;

        // caching for easier to read
        Equipment left = playerEquipment.equipment[leftWeaponIndex];
        Equipment right = playerEquipment.equipment[rightWeaponIndex];

        // find total of magical damage value
        int wisdom = 0;
        int intelligence = 0;

        // getting bonus for player's wisdom
        if(playerEquipment.equipment[leftWeaponIndex] != null)
            wisdom += (int) (left.baseMagical + (left.baseMagical * left.wisdomScaling * playerStats.GetStatBonus(playerStats.wisdom)));

        if(playerEquipment.equipment[rightWeaponIndex] != null)    
            wisdom += (int) (right.baseMagical + (right.baseMagical * right.wisdomScaling * playerStats.GetStatBonus(playerStats.wisdom)));

        
        // getting bonus for player's intelligence
        if(playerEquipment.equipment[leftWeaponIndex] != null)
            intelligence += (int) (left.baseMagical + (left.baseMagical * left.intelligenceScaling * playerStats.GetStatBonus(playerStats.intelligence)));

        if(playerEquipment.equipment[rightWeaponIndex] != null) 
            intelligence += (int) (right.baseMagical + (right.baseMagical * right.intelligenceScaling * playerStats.GetStatBonus(playerStats.intelligence)));

        
        // if dualwielding reduce the bonus to a reasonable amount
        if(playerEquipment.isDualWielding){
            wisdom -= (int)(wisdom / 1.1f);
            intelligence -= (int)(intelligence / 1.1f);
        } 

        return wisdom + intelligence;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayer();
        playerStats.strength += 35;
        playerEquipment.equipment[(int)EquipmentSlot.LeftWeapon] = ItemLoader.itemDatabase.equipmentDB[0];
        Debug.Log(GetPhysicalDamageRating());
    }
    
}
