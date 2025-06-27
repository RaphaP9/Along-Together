using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameplaySceneDataSaveLoader : SceneDataSaveLoader
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    private void OnEnable()
    {
        GameplaySessionRunDataSaveLoader.OnTriggerDataSaveOnRoundCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnTutorialCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunCompleted += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunLost += GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;
    }

    private void OnDisable()
    {
        GameplaySessionRunDataSaveLoader.OnTriggerDataSaveOnRoundCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted;

        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnTutorialCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunCompleted -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted;
        GameplaySessionPerpetualDataSaveLoader.OnTriggerDataSaveOnRunLost -= GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost;
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

    #region Subscriptions
    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRoundCompleted()
    {
        await HandleRunDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnTutorialCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task GameplaySessionPerpetualDataSaveLoader_OnTriggerDataSaveOnRunLost()
    {
        await HandlePerpetualDataSave();
    }
    #endregion
}
