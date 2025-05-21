using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private GeneralStagesManager generalStagesManager;
    [Space]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [Space]
    [SerializeField] private ObjectsInventoryManager objectsInventoryManager;
    [Space]
    [SerializeField] private RunAssetStatModifierManager runAssetStatModifierManager;
    [SerializeField] private RunNumericStatModifierManager runNumericStatModifierManager;

    //Runtime Filled
    private Transform playerTransform;

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
    }

    #region Abstract Methods

    public override void LoadRuntimeData()
    {
        LoadCurrentStageNumber();
        LoadCurrentRoundNumber();

        LoadCurrentCharacter();

        LoadPlayerCurrentHealth();
        LoadPlayerCurrentShield();

        LoadObjects();

        LoadRunNumericStats();
        LoadRunAssetStats();

        LoadCharacterAbilityLevels();
        LoadCharacterSlotsAbilityVariants();
    }

    public override void SaveRuntimeData()
    {
        SaveCurrentStageNumber();
        SaveCurrentRoundNumber();

        SavePlayerCurrentCharacter();

        SavePlayerCurrentHealth();
        SavePlayerCurrentShield();

        SaveObjects();

        SaveRunNumericStats();
        SaveRunAssetStats();

        SaveCharacterAbilityLevels();
        SaveCharacterSlotsAbilityVariants();
    }

    #endregion

    #region LoadMethods
    private void LoadCurrentStageNumber()
    {
        if (generalStagesManager == null) return;
        generalStagesManager.SetStartingStageNumber(SessionRunDataContainer.Instance.RunData.currentStageNumber);
    }

    private void LoadCurrentRoundNumber()
    {
        if (generalStagesManager == null) return;
        generalStagesManager.SetStartingRoundNumber(SessionRunDataContainer.Instance.RunData.currentRoundNumber);
    }

    private void LoadCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        playerCharacterManager.SetCharacterSO(DataUtilities.TranslateCharacterIDToCharacterSO(SessionRunDataContainer.Instance.RunData.currentCharacterID));
    }
    private void LoadObjects()
    {
        if (objectsInventoryManager == null) return;
        objectsInventoryManager.SetObjectsInventory(DataUtilities.TranslateDataModeledObjectsToObjectsIdentified(SessionRunDataContainer.Instance.RunData.objects));
    }

    private void LoadRunAssetStats()
    {
        if (runAssetStatModifierManager == null) return;
        runAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledAssetStatsToAssetStatModifiers(SessionRunDataContainer.Instance.RunData.assetStats));
    }

    private void LoadRunNumericStats()
    {
        if(runNumericStatModifierManager == null) return;
        runNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledNumericStatsToNumericStatModifiers(SessionRunDataContainer.Instance.RunData.numericStats));
    }

    private void LoadPlayerCurrentHealth()
    {
        if(playerTransform == null) return;

        PlayerHealth playerHealth =  playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;   

        playerHealth.SetCurrentHealth(SessionRunDataContainer.Instance.RunData.currentHealth);
    }

    private void LoadPlayerCurrentShield()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        playerHealth.SetCurrentShield(SessionRunDataContainer.Instance.RunData.currentShield);
    }

    private void LoadCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if(playerAbilityLevelsHandler == null) return;

        playerAbilityLevelsHandler.SetStartingAbilityLevels(DataUtilities.TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(SessionRunDataContainer.Instance.RunData.abilityLevelGroups));
    }

    private void LoadCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        playerAbilitySlotsVariantsHandler.SetStartingAbilityVariants(DataUtilities.TranslateDataModeledAbilitySlotGroupsToPrimitiveAbilitySlotGroups(SessionRunDataContainer.Instance.RunData.abilitySlotGroups));
    }
    #endregion

    #region SaveMethods

    private void SaveCurrentStageNumber()
    {
        if (generalStagesManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentStageNumber(generalStagesManager.CurrentStageNumber);
    }

    private void SaveCurrentRoundNumber()
    {
        if (generalStagesManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentRoundNumber(generalStagesManager.CurrentRoundNumber);
    }

    private void SavePlayerCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentCharacterID(playerCharacterManager.CharacterSO.id);
    }
    private void SaveObjects()
    {
        if (objectsInventoryManager == null) return;
        SessionRunDataContainer.Instance.SetObjects(DataUtilities.TranslateObjectsIdentifiedToDataModeledObjects(objectsInventoryManager.ObjectsInventory));
    }

    private void SaveRunNumericStats()
    {
        if (runNumericStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataModeledNumericStats(runNumericStatModifierManager.NumericStatModifiers));
    }

    private void SaveRunAssetStats()
    {
        if (runAssetStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetAssetStats(DataUtilities.TranslateAssetStatModifiersToDataModeledAssetStats(runAssetStatModifierManager.AssetStatModifiers));
    }

    private void SavePlayerCurrentHealth()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        SessionRunDataContainer.Instance.SetCurrentHealth(playerHealth.CurrentHealth);
    }

    private void SavePlayerCurrentShield()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponentInChildren<PlayerHealth>();

        if (playerHealth == null) return;

        SessionRunDataContainer.Instance.SetCurrentShield(playerHealth.CurrentShield);
    }

    private void SaveCharacterAbilityLevels()
    {
        if (playerTransform == null) return;

        PlayerAbilitiesLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilitiesLevelsHandler>();

        if (playerAbilityLevelsHandler == null) return;

        SessionRunDataContainer.Instance.SetAbilityLevels(DataUtilities.TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(playerAbilityLevelsHandler.GetPrimitiveAbilityLevelGroups()));
    }

    private void SaveCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;

        SessionRunDataContainer.Instance.SetAbilitySlotsVariants(DataUtilities.TranslatePrimitiveAbilitySlotGroupsToDataModeledAbilitySlotGroups(playerAbilitySlotsVariantsHandler.GetPrimitiveAbilitySlotGroups()));
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;

        if (!sceneDataSaveLoader.CanLoadDataDynamically()) return;

        LoadPlayerCurrentHealth();
        LoadPlayerCurrentShield();

        LoadCharacterAbilityLevels();
        LoadCharacterSlotsAbilityVariants();
    }
    #endregion
}
