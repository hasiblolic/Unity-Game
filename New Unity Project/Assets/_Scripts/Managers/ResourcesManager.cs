using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources Manager")]
public class ResourcesManager : ScriptableObject
{
	public BaseItem[] allItems;
	[System.NonSerialized]
	Dictionary<string, BaseItem> itemsDict = new Dictionary<string, BaseItem>();

	public void Init()
	{
		for (int i = 0; i < allItems.Length; i++)
		{
			itemsDict.Add(allItems[i].name, allItems[i]);
		}
	}

	public BaseItem GetItem(string id)
	{
		BaseItem retVal = null;
		itemsDict.TryGetValue(id, out retVal);
		return retVal;
	}
}
