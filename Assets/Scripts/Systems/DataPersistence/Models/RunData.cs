using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    public int currentCharacterID;
    [Space]
    public int currentHealth;
    public int currentShield;
    [Space]
    public List<DataModeledAssetStat> assetStats;
    public List<DataModeledNumericStat> numericStats;
    [Space]
    public List<DataModeledAbilityLevelGroup> abilityLevelGroups;
    public List<DataModeledAbilitySlotGroup> abilitySlotGroups;

    public RunData()
    {
        currentCharacterID = 0;

        currentHealth = 0;
        currentShield = 0;

        numericStats = new List<DataModeledNumericStat>();
        assetStats = new List<DataModeledAssetStat>(); 

        abilityLevelGroups = new List<DataModeledAbilityLevelGroup>();
        abilitySlotGroups = new List<DataModeledAbilitySlotGroup>();
    }
}
