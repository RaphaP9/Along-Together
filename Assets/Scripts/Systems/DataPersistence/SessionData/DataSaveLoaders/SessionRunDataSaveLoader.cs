using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts")]
    [SerializeField] private RunAssetStatModifierManager runAssetStatModifierManager;
    [SerializeField] private RunNumericStatModifierManager runNumericStatModifierManager;
    [SerializeField] private PlayerHealth playerHealth;

    public override void LoadRuntimeData()
    {
        LoadCurrentHealth();
        LoadCurrentShield();
        LoadRunNumericStats();
        LoadRunAssetStats();
    }

    public override void SaveRuntimeData()
    {
        SaveCurrentHealth();
        SaveCurrentShield();
        SaveRunNumericStats();
        SaveRunAssetStats();
    }

    #region LoadMethods
    private void LoadRunAssetStats()
    {
        if (runAssetStatModifierManager == null) return;
        runAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(SessionRunDataContainer.Instance.RunData.assetStats));
    }

    private void LoadRunNumericStats()
    {
        if(runNumericStatModifierManager == null) return;
        runNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionRunDataContainer.Instance.RunData.numericStats));
    }

    private void LoadCurrentHealth()
    {
        if(playerHealth == null) return;
        playerHealth.SetCurrentHealth(SessionRunDataContainer.Instance.RunData.currentHealth);
    }

    private void LoadCurrentShield()
    {
        if (playerHealth == null) return;
        playerHealth.SetCurrentShield(SessionRunDataContainer.Instance.RunData.currentShield);
    }
    #endregion

    #region SaveMethods

    private void SaveCurrentHealth()
    {
        if (playerHealth == null) return;
        SessionRunDataContainer.Instance.SetCurrentHealth(playerHealth.CurrentHealth);
    }

    private void SaveCurrentShield()
    {
        if (playerHealth == null) return;
        SessionRunDataContainer.Instance.SetCurrentShield(playerHealth.CurrentShield);
    }

    private void SaveRunNumericStats()
    {
        if (runNumericStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataPersistentNumericStats(runNumericStatModifierManager.NumericStatModifiers));
    }

    private void SaveRunAssetStats()
    {
        if (runAssetStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetAssetStats(DataUtilities.TranslateAssetStatModifiersToDataPersistentAssetStats(runAssetStatModifierManager.AssetStatModifiers));
    }


    #endregion
}
