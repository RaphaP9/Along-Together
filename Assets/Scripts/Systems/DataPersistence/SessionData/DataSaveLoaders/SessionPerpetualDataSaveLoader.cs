using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPerpetualDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts")]
    [SerializeField] private PerpetualAssetStatModifierManager perpetualAssetStatModifierManager;
    [SerializeField] private PerpetualNumericStatModifierManager perpetualNumericStatModifierManager;

    public override void LoadRuntimeData()
    {
        LoadRunNumericStats();
        LoadRunAssetStats();
    }

    public override void SaveRuntimeData()
    {
        SaveRunNumericStats();
        SaveRunAssetStats();
    }


    #region LoadMethods
    private void LoadRunAssetStats()
    {
        if (perpetualAssetStatModifierManager == null) return;
        perpetualAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.assetStats));
    }

    private void LoadRunNumericStats()
    {
        if(perpetualNumericStatModifierManager == null) return;
        perpetualNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.numericStats));
    }
    #endregion


    #region SaveMethods
    private void SaveRunNumericStats()
    {
        if (perpetualNumericStatModifierManager == null) return;
        SessionPerpetualDataContainer.Instance.PerpetualData.numericStats = DataUtilities.TranslateNumericStatModifiersToDataPersistentNumericStats(perpetualNumericStatModifierManager.NumericStatModifiers);
    }

    private void SaveRunAssetStats()
    {
        if (perpetualAssetStatModifierManager == null) return;
        SessionPerpetualDataContainer.Instance.PerpetualData.assetStats = DataUtilities.TranslateAssetStatModifiersToDataPersistentAssetStats(perpetualAssetStatModifierManager.AssetStatModifiers);
    }
    #endregion
}
