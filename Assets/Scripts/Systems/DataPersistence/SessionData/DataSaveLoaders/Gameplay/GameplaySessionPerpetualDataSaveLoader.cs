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
    public override void InjectAllDataFromDataContainers()
    {

    }

    public override void ExtractAllDataToDataContainers()
    {
        SaveHasCompletedTutorial();
    }
    #endregion

    #region LoadMethods

    #endregion

    #region SaveMethods
    private void SaveHasCompletedTutorial()
    {
        if(GeneralStagesManager.Instance == null) return;
        if(GameManager.Instance == null) return;
        if(StageEventsDefiner.Instance == null) return;

        //Stage Events Definer Checks if tutorial has been completed
        if (StageEventsDefiner.Instance.IsTutorialCompleted()) SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(true);
        else SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(false);
    }
    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;
    }
    #endregion
}
