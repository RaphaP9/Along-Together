using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionRunDataContainer : MonoBehaviour
{
    public static SessionRunDataContainer Instance { get; private set; }

    [Header("Data")]
    [SerializeField] private RunData runData = new();

    public RunData RunData => runData;

    #region Singleton Initialization
    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    public void SetRunData(RunData runData) => this.runData = runData;
    public void ClearRunData() => runData = new();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetCurrentStageNumber(int stageNumber) => runData.currentStageNumber = stageNumber;
    public void SetCurrentRoundNumber(int roundNumber) => runData.currentRoundNumber = roundNumber;

    public void SetCurrentCharacterID(int characterID) => runData.currentCharacterID = characterID;

    public void SetCurrentHealth(int currentHealth) => runData.currentHealth = currentHealth;
    public void SetCurrentShield(int currentShield) => runData.currentShield = currentShield;

    public void SetNumericStats(List<DataModeledNumericStat> dataModeledNumericStats) => runData.numericStats = dataModeledNumericStats;
    public void SetAssetStats(List<DataModeledAssetStat> dataModeledAssetStats) => runData.assetStats = dataModeledAssetStats;

    public void SetAbilityLevels(List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups) => runData.abilityLevelGroups = dataModeledAbilityLevelGroups;
    public void SetAbilitySlotsVariants(List<DataModeledAbilitySlotGroup> dataModeledAbilitySlotGroups) => runData.abilitySlotGroups = dataModeledAbilitySlotGroups;
}
