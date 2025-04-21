using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int currentHealth;
    public int currentShield;

    public List<DataModeledAssetStat> assetStats;
    public List<DataModeledNumericStat> numericStats;

    public RunData()
    {
        currentHealth = 0;
        currentShield = 0;

        numericStats = new List<DataModeledNumericStat>();
        assetStats = new List<DataModeledAssetStat>(); 
    }
}
