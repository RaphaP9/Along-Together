using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySessionRunDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
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
        LoadCurrentCharacter();

        LoadPlayerCurrentHealth();
        LoadPlayerCurrentShield();

        LoadRunNumericStats();
        LoadRunAssetStats();

        LoadCharacterAbilityLevels();
        LoadCharacterSlotsAbilityVariants();
    }

    public override void SaveRuntimeData()
    {
        SavePlayerCurrentCharacter();

        SavePlayerCurrentHealth();
        SavePlayerCurrentShield();

        SaveRunNumericStats();
        SaveRunAssetStats();

        SaveCharacterAbilityLevels();
        SaveCharacterSlotsAbilityVariants();
    }

    #endregion

    #region LoadMethods
    private void LoadCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        playerCharacterManager.SetCharacterSO(DataUtilities.TranslateCharacterIDToCharacterSO(SessionRunDataContainer.Instance.RunData.currentCharacterID));
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

        PlayerAbilityLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilityLevelsHandler>();

        if(playerAbilityLevelsHandler == null) return;

        playerAbilityLevelsHandler.SetStartingAbilityLevels(DataUtilities.TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(SessionRunDataContainer.Instance.RunData.abilityLevelGroups));
    }

    private void LoadCharacterSlotsAbilityVariants()
    {
        if (playerTransform == null) return;

        PlayerAbilitySlotsVariantsHandler playerAbilitySlotsVariantsHandler = playerTransform.GetComponentInChildren<PlayerAbilitySlotsVariantsHandler>();

        if(playerAbilitySlotsVariantsHandler == null) return;  
    }
    #endregion

    #region SaveMethods

    private void SavePlayerCurrentCharacter()
    {
        if (playerCharacterManager == null) return;
        SessionRunDataContainer.Instance.SetCurrentCharacterID(playerCharacterManager.CharacterSO.id);
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

        PlayerAbilityLevelsHandler playerAbilityLevelsHandler = playerTransform.GetComponentInChildren<PlayerAbilityLevelsHandler>();

        if (playerAbilityLevelsHandler == null) return;

        SessionRunDataContainer.Instance.SetAbilityLevels(DataUtilities.TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(playerAbilityLevelsHandler.GetPrimitiveAbilityLevelGroups()));
    }

    private void SaveCharacterSlotsAbilityVariants()
    {

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
