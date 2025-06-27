using System.Collections;
using System.Collections.Generic;
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

    public async void HandleDataSaveMidRounds() //Async Methods!, Public (Called by other classes)
    {
        await GeneralDataSaveLoader.Instance.CompleteDataSaveAsync();
    }

    #region Subscriptions
    private void GameManager_OnTriggerDataSaveOnRoundEnd(object sender, System.EventArgs e)
    {
        HandleDataSaveMidRounds();
    }
    private void WinManager_OnTriggerDataSaveOnRunCompleted(object sender, System.EventArgs e)
    {
        //Should save dataa with new unlocked Characters
    }

    private void LoseManager_OnTriggerDataSaveOnRunLost(object sender, System.EventArgs e)
    {
        //
    }

    #endregion
}
