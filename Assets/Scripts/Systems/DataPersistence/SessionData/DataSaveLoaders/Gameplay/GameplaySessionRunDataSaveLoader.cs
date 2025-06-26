using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private GameManager gameManager;
    [Space]
    [SerializeField] private GeneralStagesManager generalStagesManager;
    [Space]
    [SerializeField] private GoldManager goldManager;   
    [Space]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [Space]
    [SerializeField] private ObjectsInventoryManager objectsInventoryManager;
    [Space]
    [SerializeField] private TreatsInventoryManager treatsInventoryManager;
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
        LoadTutorializedRunBoolean();

        LoadCurrentStageNumber();
        LoadCurrentRoundNumber();

        LoadCurrentGold();

        LoadCurrentCharacter();

        LoadPlayerCurrentHealth();
        LoadPlayerCurrentShield();

        LoadObjects();
        LoadTreats();

        LoadRunNumericStats();

        LoadCharacterAbilityLevels();
        LoadCharacterSlotsAbilityVariants();
    }

    public override void SaveRuntimeData()
    {
        SaveTutorializedRunBoolean();

        SaveCurrentStageNumber();
        SaveCurrentRoundNumber();

        SaveCurrentGold();

        SavePlayerCurrentCharacter();

        SavePlayerCurrentHealth();
        SavePlayerCurrentShield();

        SaveObjects();
        SaveTreats();

        SaveRunNumericStats();

        SaveCharacterAbilityLevels();
        SaveCharacterSlotsAbilityVariants();
    }

    #endregion

    #region LoadMethods
    private void LoadTutorializedRunBoolean()
    {
        if (gameManager == null) return;
        gameManager.SetTutorializedRun(SessionRunDataContainer.Instance.RunData.tutorializedRun);
    }

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

    private void LoadCurrentGold()
    {
        if (goldManager == null) return;
        goldManager.SetCurrentGold(SessionRunDataContainer.Instance.RunData.currentGold);
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

    private void LoadTreats()
    {
        if (treatsInventoryManager == null) return;
        treatsInventoryManager.SetTreatsInventory(DataUtilities.TranslateDataModeledTreatsToTreatsIdentified(SessionRunDataContainer.Instance.RunData.treats));
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

    private void SaveTutorializedRunBoolean()
    {
        if (gameManager == null) return;
        SessionRunDataContainer.Instance.SetTutorializedRunBoolean(gameManager.TutorializedRun);
    }
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

    private void SaveCurrentGold()
    {
        if(goldManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentGold(goldManager.CurrentGold);
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

    private void SaveTreats()
    {
        if (treatsInventoryManager == null) return;
        SessionRunDataContainer.Instance.SetTreats(DataUtilities.TranslateTreatsIdentifiedToDataModeledTreats(treatsInventoryManager.TreatsInventory));
    }

    private void SaveRunNumericStats()
    {
        if (runNumericStatModifierManager == null) return;
        SessionRunDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataModeledNumericStats(runNumericStatModifierManager.NumericStatModifiers));
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
