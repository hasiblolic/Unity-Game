﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    static ResourcesManager _resourcesManager;

    public static ResourcesManager resourcesManager
    {
        get
        {
            if(_resourcesManager == null)
            {
                _resourcesManager = Resources.Load("Resources Manager") as ResourcesManager;
                _resourcesManager.Init();
            }

            return _resourcesManager;
        }
    }
}
