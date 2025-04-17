using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerpetualData 
{
    public List<DataPersistentAssetStat> assetStats;
    public List<DataPersistentNumericStat> numericStats;

    public PerpetualData()
    {
        numericStats = new List<DataPersistentNumericStat>();
        assetStats = new List<DataPersistentAssetStat>();
    }
}
