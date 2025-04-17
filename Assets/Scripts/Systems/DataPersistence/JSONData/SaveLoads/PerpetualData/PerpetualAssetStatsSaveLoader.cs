using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualAssetStatsSaveLoader : MonoBehaviour, IDataSaveLoader<PerpetualData>
{
    public void LoadData(PerpetualData data)
    {
        SessionDataManager.Instance.PerpetualData.assetStats = data.assetStats;
    }

    public void SaveData(ref PerpetualData data)
    {
        data.assetStats = SessionDataManager.Instance.PerpetualData.assetStats;
    }
}
