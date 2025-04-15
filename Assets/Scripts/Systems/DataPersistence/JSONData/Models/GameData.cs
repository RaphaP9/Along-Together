using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public List<DataPersistentAssetStat> dataPersistentAssetStats;
    public List<DataPersistentNumericStat> dataPersistentNumericStats;

    public GameData()
    {
        dataPersistentNumericStats = new List<DataPersistentNumericStat>();
        dataPersistentAssetStats = new List<DataPersistentAssetStat>();
    }
}
