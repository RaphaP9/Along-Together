using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAssetStatsSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        SessionDataManager.Instance.RunData.assetStats = data.assetStats;
    }

    public void SaveData(ref RunData data)
    {
        data.assetStats = SessionDataManager.Instance.RunData.assetStats;
    }
}
