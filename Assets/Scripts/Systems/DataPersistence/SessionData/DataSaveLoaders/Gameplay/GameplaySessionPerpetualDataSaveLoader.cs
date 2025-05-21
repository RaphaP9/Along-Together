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
