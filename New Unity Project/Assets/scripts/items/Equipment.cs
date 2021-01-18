using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Equipment : Item
{   
    [XmlAttribute("equipmentSlot")]
    public EquipmentSlot equipmentSlot;

    [XmlAttribute("basePhysical")]
    public int basePhysical;

    [XmlAttribute("baseMagical")]
    public int baseMagical;

    [XmlAttribute("strengthScaling")]
    public float strengthScaling;

    [XmlAttribute("dexterityScaling")]
    public float dexterityScaling;

    [XmlAttribute("intelligenceScaling")]
    public float intelligenceScaling;

    [XmlAttribute("wisdomScaling")]
    public float wisdomScaling;

    public override void Use() {
        base.Use();
        
    }
}
