using UnityEngine;
using System.Collections;

public static class GameSettings
{
	static ResourcesManager _resourcesManager;
	public static ResourcesManager resourcesManager
	{
		get
		{
			if (_resourcesManager == null)
			{
				_resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
				_resourcesManager.Init();
			}

			return _resourcesManager;
		}
	}
}
