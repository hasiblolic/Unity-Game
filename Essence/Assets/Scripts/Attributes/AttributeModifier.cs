using UnityEngine;
using System.Collections;

public enum AttributeModType
{
    Flat = 100,
    PercentageAdd = 200,
    PercentageMultiply = 300,
}

[System.Serializable]
public class AttributeModifier
{
    public readonly float Value;
    public readonly AttributeModType Type;
    public readonly int Order;
    public readonly object Source;

    public AttributeModifier(float value, AttributeModType modType, int order, object source)
    {
        Value = value;
        Type = modType;
        Order = order;
        Source = source;
    }

    public AttributeModifier(float value, AttributeModType type) : this(value, type, (int)type, null) { }
    public AttributeModifier(float value, AttributeModType type, int order) : this(value, type, order, null) { }
    public AttributeModifier(float value, AttributeModType type, object source) : this(value, type, (int)type, source) { }

}
