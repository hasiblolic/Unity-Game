using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attributes/Character Attributes")]
public class CharacterAttributes : ScriptableObject
{
    [Header("Vitals")]
    public VitalsAttribute health;
    public VitalsAttribute mana;
    public VitalsAttribute stamina;

    [Header("Attributes")]
    public int level;
    public int attributePoints;

    public Attribute strength;
    public Attribute dexterity;
    
    public Attribute intelligence;
    public Attribute faith;

    public Attribute endurance;
    public Attribute vitality;
    public Attribute resistance;

    public Attribute humanity;

    [Header("Secondary Attributes")]
    private Attribute equipmentLoad;


    #region Secondary Attribute Related

    public bool IsOverWeight(Controller c)
    {
        // find weight of all equipment you have
        // compare with equip load attribute
        return false;
    }

    public bool IsOverWeight(AIController ai)
    {
        return false;
    }

    #endregion

    public void SetDefaultAttributes()
    {
        health.SetDependantAttribute(vitality);
        mana.SetDependantAttribute(intelligence);
        stamina.SetDependantAttribute(endurance);
    }
}
