using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public List<DataPersistentAssetStat> dataPersistentAssetStats;
    public List<DataPersistentNumericStat> dataPersistentNumericStats;

    public RunData()
    {
        dataPersistentNumericStats = new List<DataPersistentNumericStat>();
        dataPersistentAssetStats = new List<DataPersistentAssetStat>(); 
    }
}
