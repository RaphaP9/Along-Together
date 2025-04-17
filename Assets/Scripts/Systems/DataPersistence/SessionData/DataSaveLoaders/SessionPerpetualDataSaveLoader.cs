using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPerpetualDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts")]
    [SerializeField] private PerpetualAssetStatModifierManager perpetualAssetStatModifierManager;
    [SerializeField] private PerpetualNumericStatModifierManager perpetualNumericStatModifierManager;

    protected override void LoadRuntimeData()
    {
        LoadRunAssetStats();
        LoadRunNumericStats();
    }

    protected override void SaveRuntimeData()
    {

    }

    #region LoadMethods
    private void LoadRunAssetStats() => perpetualAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.assetStats));
    private void LoadRunNumericStats() => perpetualNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.numericStats));
    #endregion

    #region SaveMethods

    #endregion
}
