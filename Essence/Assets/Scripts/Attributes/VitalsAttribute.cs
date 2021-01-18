using UnityEngine;
using System.Collections;

[System.Serializable]
public class VitalsAttribute : Attribute
{
    protected Attribute dependantAttribute;

    private float currentValue;
    public float CurrentValue
    {
        get
        {
            if (currentValue < 0)
                currentValue = 0;
            else if (currentValue > Value)
                currentValue = Value;
            return currentValue;
        }
        set
        {
            currentValue = value;
        }
    }


    protected override float CalculateFinalValue()
    {
        float finalValue = 0;
        float sumPercentAdd = 0;
        float bonusFactor = 0;
        float magnifier = 0;

        
        if (dependantAttribute.Value < 20)
            magnifier = 0.42f;
        else if (dependantAttribute.Value < 50)
            magnifier = 0.5f;
        else if (dependantAttribute.Value < 55)
            magnifier = 0.49f;
        else if (dependantAttribute.Value < 60)
            magnifier = 0.47f;
        else if (dependantAttribute.Value == 60)
            magnifier = 0.455f;
        else if (dependantAttribute.Value < 65)
            magnifier = 0.45f;
        else if (dependantAttribute.Value < 70)
            magnifier = 0.43f;
        else if (dependantAttribute.Value < 75)
            magnifier = 0.425f;
        else if (dependantAttribute.Value < 80)
            magnifier = 0.415f;
        else if (dependantAttribute.Value < 90)
            magnifier = 0.41f;
        else magnifier = 0.408f;

        bonusFactor = (Mathf.Sin((dependantAttribute.Value / 100) * magnifier * Mathf.PI) / 100);
        finalValue = baseValue * (bonusFactor * baseValue) + (baseValue - 100);

        // after value of attribute has been calculated based off attributes, add in the modifiers
        for (int i = 0; i < modifiers.Count; i++)
        {
            AttributeModifier mod = modifiers[i];
            if (mod.Type == AttributeModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == AttributeModType.PercentageAdd)
            {
                sumPercentAdd += mod.Value;
                if (i + 1 >= modifiers.Count || modifiers[i + 1].Type != AttributeModType.PercentageAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
                else if (mod.Type == AttributeModType.PercentageMultiply)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
        }
        return (float)System.Math.Round(finalValue, 4);
    }

    public void SetDependantAttribute(Attribute dependant)
    {
        dependantAttribute = dependant;
    }
}
