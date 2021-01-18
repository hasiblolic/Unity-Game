using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be on an empty gameObject and will hold all of the items in our database in it readily available
public class ItemLoader : MonoBehaviour
{
    public TextAsset XML_DATABASE;
    public static ItemDatabase itemDatabase;

    // Start is called before the first frame update
    void Start()
    {
        itemDatabase = ItemDatabase.Load(XML_DATABASE);
        DontDestroyOnLoad(this);
    }
}
