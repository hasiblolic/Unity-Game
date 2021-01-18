using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Cloth Item")]
public class ClothItem : BaseItem
{
    // what type of item this is
    public ClothItemType clothType;

    // the mesh of the item
    public Mesh mesh;

    // the material the item will have
    public Material clothMaterial;
}
