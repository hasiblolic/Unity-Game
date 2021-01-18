using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    [Header("Weapon Values")]
    public GameObject itemModel;
    public int basePhysical;
    public int baseMagic;
    public int baseFire;
    public int baseLightning;
    public int baseChaos;
    public bool isTwoHanded;

    [Header("Requirements")]
    public int minStr;
    public int minDex;
    public int minFaith;
    public int minInt;

    [HideInInspector] public ParamBonus strength;
    [HideInInspector] public ParamBonus dexterity;
    [HideInInspector] public ParamBonus faith;
    [HideInInspector] public ParamBonus intelligence;

    public AttributeModifier[] attributeModifiers;

    public float TotalAttackRating(Controller controller)
    {
        float retVal = 0;
        retVal += PhysicalRating(controller);
        retVal += MagicRating(controller);
        retVal += FireRating(controller);
        retVal += LightningRating(controller);
        retVal += ChaosRating(controller);
        return retVal;
    }

    // PhysicalRating = BasePhysical + BonusStrength + BonusDexterity + BonusChaosPhysical
    public float PhysicalRating(Controller controller)
    {
        float retVal = 0;
        retVal += basePhysical;

        // Bonus Strength = BasePhysical × StrengthScaling × StrengthRating
        retVal += basePhysical * strength.scaling * controller.stats.strength.Value;

        // Bonus Dexterity = BasePhysical × DexterityScaling × DexterityRating
        retVal += basePhysical * dexterity.scaling * controller.stats.dexterity.Value;

        // BonusChaosPhysical = (BasePhysical + BonusStrength + BonusDexterity) × HumanityScaling × HumanityRatingPhysical


        return retVal;
    }

    // MagicRating = BaseMagic + BonusIntelligence + BonusFaith
    public float MagicRating(Controller controller)
    {
        float retVal = 0;
        retVal += baseMagic;

        // BonusIntelligence = BaseMagic × IntelligenceScaling × IntelligenceRating
        retVal += baseMagic * intelligence.scaling * controller.stats.intelligence.Value;

        // BonusFaith = BaseMagic × FaithScaling × FaithRating
        retVal += baseMagic * faith.scaling * controller.stats.faith.Value;

        return retVal;
    }

    // FireRating = BaseFire + BonusChaosFire
    public float FireRating(Controller controller)
    {
        // BonusChaosFire = BaseFire × HumanityScaling × HumanityRatingFire
        return baseFire;
    }

    public float LightningRating(Controller controller)
    {
        return baseLightning;
    }

    public float ChaosRating(Controller controller)
    {
        return baseChaos;
    }

    public void EquipWeapon()
    {

    }
}

[System.Serializable]
public class ParamBonus
{
    public ScalingValues scalingRating;
    public float scaling;
}



/*
    TotalAttackRating = PhysicalRating + MagicRating + FireRating + LightningRating

    PhysicalRating = BasePhysical + BonusStrength + BonusDexterity + BonusChaosPhysical
    BonusStrength = BasePhysical × StrengthScaling × StrengthRating
    BonusDexterity = BasePhysical × DexterityScaling × DexterityRating
    BonusChaosPhysical = (BasePhysical + BonusStrength + BonusDexterity) × HumanityScaling × HumanityRatingPhysical

    MagicRating = BaseMagic + BonusIntelligence + BonusFaith
    BonusIntelligence = BaseMagic × IntelligenceScaling × IntelligenceRating
    BonusFaith = BaseMagic × FaithScaling × FaithRating

    FireRating = BaseFire + BonusChaosFire
    BonusChaosFire = BaseFire × HumanityScaling × HumanityRatingFire
    LightningRating = BaseLightning


    Dam = 0.4*(AR^3/ Def^2) - 0.09*(AR^2/ Def)+0.1*Atk (when AR < Def)


    Dam = 0.4*AR (when AR = Def)


    Dam = AR - 0.79* Def*e^(-0.27* Def/AR) (when AR > Def)

*/
