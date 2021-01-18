using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothItemHook : MonoBehaviour
{
    public ClothItemType clothItemType;
    public SkinnedMeshRenderer meshRenderer;
    public Mesh defaultMesh;
    public Material defaultMaterial;


    // this will find a reference to the clothmanager and register itself as a hook
    public void Init()
    {
        ClothManager clothManager = GetComponentInParent<ClothManager>();
        clothManager.RegisterClothHook(this);
    }

    // this will load our item onto our mesh
    internal void LoadClothItem(ClothItem clothItem)
    {
        meshRenderer.sharedMesh = clothItem.mesh;
        meshRenderer.material = clothItem.clothMaterial;
        meshRenderer.enabled = true;
    }

    // this will disable the item and return to the default mesh
    internal void UnloadItem()
    {
        if (clothItemType.isDisabledWhenNoItem)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            meshRenderer.sharedMesh = defaultMesh;
            meshRenderer.material = defaultMaterial;
        }
    }
}
