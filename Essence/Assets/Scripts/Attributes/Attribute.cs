using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attribute
{
    protected float lastBaseValue = 0;
    protected bool isDirty = true;
    protected float _value;
    public float baseValue;
    public virtual float Value 
    {
        get
        {
            lastBaseValue = baseValue;
            _value = CalculateFinalValue();
            isDirty = false;

            return _value;
        }
    }
    protected readonly List<AttributeModifier> modifiers;
    public readonly ReadOnlyCollection<AttributeModifier> attributeModifiers;

    public Attribute()
    {
        modifiers = new List<AttributeModifier>();
        attributeModifiers = modifiers.AsReadOnly();
    }

    public Attribute(float bValue) : this()
    {
        baseValue = bValue;
    }

    public virtual void AddModifier(AttributeModifier modifier)
    {
        isDirty = true;
        modifiers.Add(modifier);
        modifiers.Sort(CompareModifierOrder);
    }

    public virtual bool RemoveModifier(AttributeModifier modifier)
    {
        if (modifiers.Remove(modifier))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    protected virtual int CompareModifierOrder(AttributeModifier a, AttributeModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < modifiers.Count; i++)
        {
            AttributeModifier mod = modifiers[i];
            if(mod.Type == AttributeModType.Flat)
            {
                finalValue += mod.Value;
            } 
            else if(mod.Type == AttributeModType.PercentageAdd)
            {
                sumPercentAdd += mod.Value;
                if(i + 1 >= modifiers.Count || modifiers[i+1].Type != AttributeModType.PercentageAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
                else if(mod.Type == AttributeModType.PercentageMultiply)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
        }

        return (float)System.Math.Round(finalValue, 4);
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            if(modifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                modifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

}