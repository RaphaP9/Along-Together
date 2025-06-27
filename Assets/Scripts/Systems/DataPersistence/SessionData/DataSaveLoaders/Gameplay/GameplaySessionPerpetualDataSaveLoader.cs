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

    private void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnRoundCompleted += GameManager_OnRoundCompleted;
        GameManager.OnTutorialCompleted += GameManager_OnTutorialCompleted;

        WinManager.OnRunCompleted += WinManager_OnRunCompleted;
        LoseManager.OnRunLost += LoseManager_OnRunLost;
    }

    private void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;

        GameManager.OnRoundCompleted -= GameManager_OnRoundCompleted;
        GameManager.OnTutorialCompleted -= GameManager_OnTutorialCompleted;

        WinManager.OnRunCompleted -= WinManager_OnRunCompleted;
        LoseManager.OnRunLost -= LoseManager_OnRunLost;
    }

    #region Abstract Methods
    public override void InjectAllDataFromDataContainers()
    {
        //
    }

    public override void ExtractAllDataToDataContainers()
    {
        //
    }
    #endregion

    #region LoadMethods

    #endregion


    #region PlayerSubscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        playerTransform = e.playerTransform;
    }
    #endregion

    #region Data Update Subscriptions
    private void GameManager_OnRoundCompleted(object sender, GameManager.OnRoundCompletedEventArgs e)
    {
        //
    }

    private void GameManager_OnTutorialCompleted(object sender, System.EventArgs e)
    {
        SessionPerpetualDataContainer.Instance.SetHasCompletedTutorial(true);
    }
    private void WinManager_OnRunCompleted(object sender, WinManager.OnRunCompletedEventArgs e)
    {
        SessionPerpetualDataContainer.Instance.AddUnlockedCharacterIDs(GeneralGameSettings.Instance.GetRunCompletedUnlockedCharacterIDsByCharacterSO(e.characterSO));
    }

    private void LoseManager_OnRunLost(object sender, LoseManager.OnRunLostEventArgs e)
    {
        //
    }

    #endregion
}
