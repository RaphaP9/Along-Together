using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySessionPerpetualDataSaveLoader : SessionDataSaveLoader
{
    [Header("Data Scripts - Already On Scene")]
    [SerializeField] private PerpetualAssetStatModifierManager perpetualAssetStatModifierManager;
    [SerializeField] private PerpetualNumericStatModifierManager perpetualNumericStatModifierManager;

    //Runtime Filled
    private Transform playerTransform;

    private const int MIN_STAGE_TO_COMPLETE_TUTORIAL = 1;
    private const int MIN_ROUND_TO_COMPLETE_TUTORIAL = 3;

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
        LoadPerpetualNumericStats();
        LoadPerpetualAssetStats();
    }

    public override void SaveRuntimeData()
    {
        SaveHasCompletedTutorial();

        SavePerpetualNumericStats();
        SavePerpetualAssetStats();
    }
    #endregion

    #region LoadMethods
    private void LoadPerpetualAssetStats()
    {
        if (perpetualAssetStatModifierManager == null) return;
        perpetualAssetStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledAssetStatsToAssetStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.assetStats));
    }

    private void LoadPerpetualNumericStats()
    {
        if(perpetualNumericStatModifierManager == null) return;
        perpetualNumericStatModifierManager.SetStatList(DataUtilities.TranslateDataModeledNumericStatsToNumericStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.numericStats));
    }
    #endregion

    #region SaveMethods
    private void SaveHasCompletedTutorial()
    {
        if(GeneralStagesManager.Instance == null) return;
        if(GameManager.Instance == null) return;

        bool isRunTutorialized = GameManager.Instance.TutorializedRun;
        int currentStage = GeneralStagesManager.Instance.CurrentStageNumber;
        int currentRound = GeneralStagesManager.Instance.CurrentRoundNumber;

        if (!isRunTutorialized)
        {
            SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(true);
        }
        else
        {
            if(currentStage >= MIN_STAGE_TO_COMPLETE_TUTORIAL && currentRound > MIN_ROUND_TO_COMPLETE_TUTORIAL) SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(true);
            else SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(false);
        }
    }

    private void SavePerpetualNumericStats()
    {
        if (perpetualNumericStatModifierManager == null) return;
        SessionPerpetualDataContainer.Instance.SetNumericStats(DataUtilities.TranslateNumericStatModifiersToDataModeledNumericStats(perpetualNumericStatModifierManager.NumericStatModifiers));
    }

    private void SavePerpetualAssetStats()
    {
        if (perpetualAssetStatModifierManager == null) return;
        SessionPerpetualDataContainer.Instance.SetAssetStats(DataUtilities.TranslateAssetStatModifiersToDataModeledAssetStats(perpetualAssetStatModifierManager.AssetStatModifiers));
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;

        if (!sceneDataSaveLoader.CanLoadDataDynamically()) return;
    }
    #endregion
}
