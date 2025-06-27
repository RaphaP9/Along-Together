using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameplaySceneDataSaveLoader : SceneDataSaveLoader
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    private void OnEnable()
    {
        GameManager.OnTriggerDataSaveOnRoundEnd += GameManager_OnTriggerDataSaveOnRoundEnd;
        WinManager.OnTriggerDataSaveOnRunCompleted += WinManager_OnTriggerDataSaveOnRunCompleted;
        LoseManager.OnTriggerDataSaveOnRunLost += LoseManager_OnTriggerDataSaveOnRunLost;
    }

    private void OnDisable()
    {
        GameManager.OnTriggerDataSaveOnRoundEnd -= GameManager_OnTriggerDataSaveOnRoundEnd;
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

    private async void HandleDataSaveMidRounds() => await GeneralDataSaveLoader.Instance.SaveAllJSONDataAsync(); //MidRounds Update both Run and Perpetual Data
    private async void HandleDataSaveOnRunCompleted() => await GeneralDataSaveLoader.Instance.SavePerpetualJSONDataAsync(); //On Win Only Update Perpetual Data
    private async void HandleDataSaveOnRunLost() => await GeneralDataSaveLoader.Instance.SavePerpetualJSONDataAsync(); //On Lose Only Update Perpetual Data

    #region Subscriptions
    private void GameManager_OnTriggerDataSaveOnRoundEnd(object sender, System.EventArgs e)
    {
        HandleDataSaveMidRounds();
    }
    private void WinManager_OnTriggerDataSaveOnRunCompleted(object sender, System.EventArgs e)
    {
        HandleDataSaveOnRunCompleted();
    }

    private void LoseManager_OnTriggerDataSaveOnRunLost(object sender, System.EventArgs e)
    {
        HandleDataSaveOnRunLost();
    }

    #endregion
}
