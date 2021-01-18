using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlInclude(typeof(Equipment))]
[System.Serializable]
public class Item
{
    [XmlAttribute("itemName")]
    public string itemName;

    [XmlAttribute("itemID")]
    public string itemID;

    [XmlAttribute("isKeyItem")]
    public bool isKeyItem;

    [XmlAttribute("description")]
    public string description;

    [XmlAttribute("itemValue")]
    public int itemValue;

    [XmlAttribute("itemRarity")]
    public ItemRarity itemRarity;

    [XmlAttribute("itemAmount")]
    public int itemAmount;

    [XmlAttribute("itemWeight")]
    public int itemWeight;

    public virtual void Use() {

    }
}

public enum ItemRarity {
    Common,
    Uncommon,
    Rare,
    Exotic,
    Legendary
}