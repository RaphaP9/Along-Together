using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStageTeleportation : MonoBehaviour
{
    private void OnEnable()
    {
        GeneralStagesManager.OnStageInitialized += GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange += GeneralStagesManager_OnStageChange;
    }

    private void OnDisable()
    {
        GeneralStagesManager.OnStageInitialized -= GeneralStagesManager_OnStageInitialized;
        GeneralStagesManager.OnStageChange -= GeneralStagesManager_OnStageChange;
    }

    private void GeneralStagesManager_OnStageChange(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        PlayerTeleporterManager.Instance.TeleportPlayerToPosition(GeneralStagesManager.Instance.GetPlayerSpawnPointPositionFromStageGroup(e.stageGroup), true);
    }

    private void GeneralStagesManager_OnStageInitialized(object sender, GeneralStagesManager.OnStageChangeEventArgs e)
    {
        PlayerTeleporterManager.Instance.TeleportPlayerToPosition(GeneralStagesManager.Instance.GetPlayerSpawnPointPositionFromStageGroup(e.stageGroup), true);
    }
}
