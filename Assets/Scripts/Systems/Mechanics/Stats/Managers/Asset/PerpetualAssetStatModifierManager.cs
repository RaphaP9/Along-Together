using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualAssetStatModifierManager : AssetStatModifierManager
{
    public static PerpetualAssetStatModifierManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PerpetualAssetStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        assetStatModifiers = DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(RuntimePerpetualData.RuntimeAssetStats);
    }

    protected override void SaveRuntimeData()
    {
        RuntimePerpetualData.RuntimeAssetStats = DataUtilities.TranslateAssetStatModifiersToDataPersistentAssetStats(assetStatModifiers);
    }
}

