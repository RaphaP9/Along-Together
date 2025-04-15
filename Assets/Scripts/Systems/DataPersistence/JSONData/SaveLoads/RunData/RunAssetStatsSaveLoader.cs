using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAssetStatsSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        RuntimeRunData.RuntimeAssetStats = data.assetStats;
    }

    public void SaveData(ref RunData data)
    {
        data.assetStats = RuntimeRunData.RuntimeAssetStats;
    }
}
