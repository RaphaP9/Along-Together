using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData : DataModel
{
    public bool tutorializedRun;
    [Space]
    public int currentStageNumber;
    public int currentRoundNumber;
    [Space]
    public int currentGold;
    [Space]
    public int currentCharacterID;
    [Space]
    public int currentHealth;
    public int currentShield;
    [Space]
    public List<DataModeledObject> objects;
    [Space]
    public List<DataModeledTreat> treats;
    [Space]
    public List<DataModeledAssetStat> assetStats;
    public List<DataModeledNumericStat> numericStats;
    [Space]
    public List<DataModeledAbilityLevelGroup> abilityLevelGroups;
    public List<DataModeledAbilitySlotGroup> abilitySlotGroups;

    public RunData()
    {
        currentStageNumber = 1;
        currentRoundNumber = 1;

        currentGold = 0;

        currentCharacterID = 1;

        currentHealth = 0;
        currentShield = 0;

        objects = new List<DataModeledObject>();

        treats = new List<DataModeledTreat>();

        numericStats = new List<DataModeledNumericStat>();
        assetStats = new List<DataModeledAssetStat>(); 

        abilityLevelGroups = new List<DataModeledAbilityLevelGroup>();
        abilitySlotGroups = new List<DataModeledAbilitySlotGroup>();
    }

    public override void Initialize()
    {
        if(GeneralGameSettings.Instance == null)
        {
            Debug.Log("GeneralGameSettings Instance is null. Can not Initialize DataModel.");
            return;
        }

        currentStageNumber = GeneralGameSettings.Instance.GetStartingStage();
        currentRoundNumber = GeneralGameSettings.Instance.GetStartingRound();

        currentGold = GeneralGameSettings.Instance.GetStartingGoldQuantity();

        currentCharacterID = GeneralGameSettings.Instance.GetDefaultCharacterID();
    }
}
