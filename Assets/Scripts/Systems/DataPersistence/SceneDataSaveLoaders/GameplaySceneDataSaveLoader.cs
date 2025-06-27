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

    #region Data Save Handlers
    private async Task HandleAllDataSave()
    {
        try
        {
            await GeneralDataSaveLoader.Instance.SaveAllJSONDataAsync();//MidRounds Update both Run and Perpetual Data
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex}");
        }
    }

    private async Task HandleRunDataSave()
    {
        try
        {
            await GeneralDataSaveLoader.Instance.SaveRunJSONDataAsync();//MidRounds Update both Run and Perpetual Data
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex}");
        }
    }

    private async Task HandlePerpetualDataSave()
    {
        try
        {
            await GeneralDataSaveLoader.Instance.SavePerpetualJSONDataAsync(); //On Win Only Update Perpetual Data
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex}");
        }
    }
    #endregion

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
