using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneDataSaveLoader : SceneDataSaveLoader
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

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
