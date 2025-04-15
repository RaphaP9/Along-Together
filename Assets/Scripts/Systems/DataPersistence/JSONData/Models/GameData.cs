using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public List<DataPersistentAssetStat> assetStats;
    public List<DataPersistentNumericStat> numericStats;

    public GameData()
    {
        numericStats = new List<DataPersistentNumericStat>();
        assetStats = new List<DataPersistentAssetStat>();
    }
}
