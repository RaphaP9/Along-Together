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
        GameManager.OnTriggerDataSaveOnRoundCompleted += GameManager_OnTriggerDataSaveOnRoundCompleted;
        GameManager.OnTriggerDataSaveOnTutorialCompleted += GameManager_OnTriggerDataSaveOnTutorialCompleted;
        WinManager.OnTriggerDataSaveOnRunCompleted += WinManager_OnTriggerDataSaveOnRunCompleted;
        LoseManager.OnTriggerDataSaveOnRunLost += LoseManager_OnTriggerDataSaveOnRunLost;
    }

    private void OnDisable()
    {
        GameManager.OnTriggerDataSaveOnRoundCompleted -= GameManager_OnTriggerDataSaveOnRoundCompleted;
        GameManager.OnTriggerDataSaveOnTutorialCompleted -= GameManager_OnTriggerDataSaveOnTutorialCompleted;
        WinManager.OnTriggerDataSaveOnRunCompleted -= WinManager_OnTriggerDataSaveOnRunCompleted;
        LoseManager.OnTriggerDataSaveOnRunLost -= LoseManager_OnTriggerDataSaveOnRunLost;
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
    private async Task GameManager_OnTriggerDataSaveOnRoundCompleted()
    {
        await HandleAllDataSave();
    }

    private async Task GameManager_OnTriggerDataSaveOnTutorialCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task WinManager_OnTriggerDataSaveOnRunCompleted()
    {
        await HandlePerpetualDataSave();
    }

    private async Task LoseManager_OnTriggerDataSaveOnRunLost()
    {
        await HandlePerpetualDataSave();
    }
    #endregion
}
