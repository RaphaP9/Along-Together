using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int currentHealth;
    public int currentShield;

    public List<DataPersistentAssetStat> assetStats;
    public List<DataPersistentNumericStat> numericStats;

    public RunData()
    {
        currentHealth = 0;
        currentShield = 0;

        numericStats = new List<DataPersistentNumericStat>();
        assetStats = new List<DataPersistentAssetStat>(); 
    }
}
