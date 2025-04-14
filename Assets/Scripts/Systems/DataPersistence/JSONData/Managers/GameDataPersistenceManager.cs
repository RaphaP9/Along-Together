using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataPersistenceManager : DataPersistenceManager<GameData>
{
    public static GameDataPersistenceManager Instance { get; private set; }

    private void OnEnable()
    {
        //CheckpointManager.OnCheckpointReached += CheckpointManager_OnCheckpointReached;
    }
    private void OnDisable()
    {
        //CheckpointManager.OnCheckpointReached -= CheckpointManager_OnCheckpointReached;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region Subscriptions

    private void CheckpointManager_OnCheckpointReached(object sender, EventArgs e)
    {
        SaveGameData();
    }
    #endregion
}