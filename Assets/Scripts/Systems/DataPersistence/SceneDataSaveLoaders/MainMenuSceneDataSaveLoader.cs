using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneDataSaveLoader : SceneDataSaveLoader
{
    public static MainMenuSceneDataSaveLoader Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}