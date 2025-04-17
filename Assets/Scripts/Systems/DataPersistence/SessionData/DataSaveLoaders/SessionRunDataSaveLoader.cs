using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts")]
    [SerializeField] private RunAssetStatModifierManager runAssetStatModifierManager;
    [SerializeField] private RunNumericStatModifierManager runNumericStatModifierManager;
    [SerializeField] private PlayerHealth playerHealth;

    protected override void LoadRuntimeData()
    {
        LoadRunAssetStats();
        LoadRunNumericStats();
        LoadCurrentHealth();
        LoadCurrentShield();
    }

    protected override void SaveRuntimeData()
    {

    }

    #region LoadMethods
    private void LoadRunAssetStats() => runAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(SessionRunDataContainer.Instance.RunData.assetStats));
    private void LoadRunNumericStats() => runNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionRunDataContainer.Instance.RunData.numericStats));
    private void LoadCurrentHealth() => playerHealth.SetCurrentHealth(SessionRunDataContainer.Instance.RunData.currentHealth);
    private void LoadCurrentShield() => playerHealth.SetCurrentShield(SessionRunDataContainer.Instance.RunData.currentShield);
    #endregion

    #region SaveMethods

    #endregion
}
