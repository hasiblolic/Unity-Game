using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

[XmlRoot("itemDatabase")]
public class ItemDatabase
{
    [XmlArray("items")]
    [XmlArrayItem("item")]
    public List<Item> itemsDB = new List<Item>();
    
    [XmlArray("equipments")]
    [XmlArrayItem("equipment")]
    public List<Equipment> equipmentDB = new List<Equipment>();

    public static ItemDatabase Load(TextAsset textAsset) {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        // reading xml from xml database
        StringReader reader = new StringReader(textAsset.text);
        // converting xml to serialized data
        ItemDatabase itemDB = serializer.Deserialize(reader) as ItemDatabase;
        reader.Close();
        return itemDB;
    }
}
