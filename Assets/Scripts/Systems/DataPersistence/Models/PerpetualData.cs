using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerpetualData 
{
    public List<DataModeledAssetStat> assetStats;
    public List<DataModeledNumericStat> numericStats;

    public PerpetualData()
    {
        numericStats = new List<DataModeledNumericStat>();
        assetStats = new List<DataModeledAssetStat>();
    }
}
