using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public List<DataPersistentAssetStat> assetStats;
    public List<DataPersistentNumericStat> numericStats;

    public RunData()
    {
        numericStats = new List<DataPersistentNumericStat>();
        assetStats = new List<DataPersistentAssetStat>(); 
    }
}
