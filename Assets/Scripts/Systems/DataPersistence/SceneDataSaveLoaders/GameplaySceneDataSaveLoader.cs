using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneDataSaveLoader : SceneDataSaveLoader
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    private void OnEnable()
    {
        GameManager.OnTriggerDataSave += GameManager_OnTriggerDataSave;
    }

    private void OnDisable()
    {
        GameManager.OnTriggerDataSave -= GameManager_OnTriggerDataSave;
    }

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

    private void GameManager_OnTriggerDataSave(object sender, System.EventArgs e)
    {
        HandleDynamicDataSave();
    }
}
