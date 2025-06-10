using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerpetualData 
{
    public List<int> unlockedCharacterIDs;

    public List<DataModeledAssetStat> assetStats;
    public List<DataModeledNumericStat> numericStats;

    public PerpetualData()
    {
        unlockedCharacterIDs = new List<int>();
        numericStats = new List<DataModeledNumericStat>();
        assetStats = new List<DataModeledAssetStat>();
    }
}
