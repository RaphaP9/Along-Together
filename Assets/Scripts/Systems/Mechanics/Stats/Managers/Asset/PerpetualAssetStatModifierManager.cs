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
        assetStatModifiers = DataUtilities.TranslateDataPersistentAssetStatsToAssetStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.assetStats); ;
    }

    protected override void SaveRuntimeData()
    {
        SessionPerpetualDataContainer.Instance.PerpetualData.assetStats = DataUtilities.TranslateAssetStatModifiersToDataPersistentAssetStats(assetStatModifiers);
    }
}

