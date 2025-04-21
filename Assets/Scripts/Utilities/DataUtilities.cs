using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;
using UnityEngine.Windows;

public static class DataUtilities 
{
    private const bool DEBUG = true;

    #region Numeric Stat Translation
    public static List<NumericStatModifier> TranslateDataPersistentNumericStatsToNumericStatModifiers(List<DataModeledNumericStat> dataPersistentNumericStats)
    {
        List<NumericStatModifier> numericStatModifiers = new List<NumericStatModifier>();

        foreach(DataModeledNumericStat dataPersistentNumericStat in dataPersistentNumericStats)
        {
            NumericStatModifier numericStatModifier = TranslateDataPersistentNumericStatToNumericStatModifier(dataPersistentNumericStat);
            if (numericStatModifier == null) continue;
            numericStatModifiers.Add(numericStatModifier);  
        }

        return numericStatModifiers;
    }
    public static NumericStatModifier TranslateDataPersistentNumericStatToNumericStatModifier(DataModeledNumericStat dataPersistentNumericStat)
    {
        NumericStatModifier numericStatModifier = new NumericStatModifier();

        numericStatModifier.originGUID = dataPersistentNumericStat.originGUID;

        if (Enum.TryParse<NumericStatType>(dataPersistentNumericStat.numericStatType, true, out var numericStatType)) numericStatModifier.numericStatType = numericStatType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataPersistentNumericStat.numericStatType}");
            return null;
        }

        if (Enum.TryParse<NumericStatModificationType>(dataPersistentNumericStat.numericStatModificationType, true, out var numericStatModificationType)) numericStatModifier.numericStatModificationType = numericStatModificationType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataPersistentNumericStat.numericStatModificationType}");
            return null;
        }

        numericStatModifier.value = dataPersistentNumericStat.value;

        return numericStatModifier;
    }

    //

    public static List<DataModeledNumericStat> TranslateNumericStatModifiersToDataPersistentNumericStats(List<NumericStatModifier> numericStatModifiers)
    {
        List<DataModeledNumericStat> dataPersistentNumericStats = new List<DataModeledNumericStat>();

        foreach(NumericStatModifier numericStatModifier in numericStatModifiers)
        {
            DataModeledNumericStat dataPersistentNumericStat = TranslateNumericStatModifierToDataPersistentNumericStat(numericStatModifier);
            if (dataPersistentNumericStat == null) continue;
            dataPersistentNumericStats.Add(dataPersistentNumericStat);
        }

        return dataPersistentNumericStats;
    }

    public static DataModeledNumericStat TranslateNumericStatModifierToDataPersistentNumericStat(NumericStatModifier numericStatModifier)
    {
        string originGUID = numericStatModifier.originGUID;
        string numericStatType = numericStatModifier.numericStatType.ToString();
        string numericStatModificationType = numericStatModifier.numericStatModificationType.ToString();
        float value = numericStatModifier.value;    

        DataModeledNumericStat dataPersistentNumericStat = new DataModeledNumericStat(originGUID,numericStatType,numericStatModificationType,value);
        return dataPersistentNumericStat;
    }

    #endregion

    #region Asset Stat Translation
    public static List<AssetStatModifier> TranslateDataPersistentAssetStatsToAssetStatModifiers(List<DataModeledAssetStat> dataPersistentAssetStats)
    {
        List<AssetStatModifier> assetStatModifiers = new List<AssetStatModifier>();

        foreach (DataModeledAssetStat dataPersistentAssetStat in dataPersistentAssetStats)
        {
            AssetStatModifier assetStatModifier = TranslateDataPersistentAssetStatToAssetStatModifier(dataPersistentAssetStat);
            if (assetStatModifier == null) continue;
            assetStatModifiers.Add(assetStatModifier);
        }

        return assetStatModifiers;
    }
    public static AssetStatModifier TranslateDataPersistentAssetStatToAssetStatModifier(DataModeledAssetStat dataPersistentAssetStat)
    {
        AssetStatModifier assetStatModifier = new AssetStatModifier();

        assetStatModifier.originGUID = dataPersistentAssetStat.originGUID;

        if (Enum.TryParse<AssetStatType>(dataPersistentAssetStat.assetStatType, true, out var assetStatType)) assetStatModifier.assetStatType = assetStatType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataPersistentAssetStat.assetStatType}");
            return null;
        }

        if (Enum.TryParse<AssetStatModificationType>(dataPersistentAssetStat.assetStatModificationType, true, out var assetStatModificationType)) assetStatModifier.assetStatModificationType = assetStatModificationType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataPersistentAssetStat.assetStatModificationType}");
            return null;
        }

        //Now we will translate the ID according to the AssetStatType

        switch (assetStatModifier.assetStatType)
        {
            case AssetStatType.MovementType:
            default:
                #region MovementType Translation
                if (MovementTypeAssetLibrary.Instance == null)
                {
                    if (DEBUG) Debug.Log("MovementTypeAssetLibrary is null. Can not resolve MovementTypeSO Asset");
                    return null;
                }
                assetStatModifier.asset = MovementTypeAssetLibrary.Instance.GetMovementTypeSOByID(dataPersistentAssetStat.assetID);
                #endregion
                break;
        }

        if(assetStatModifier.asset == null)
        {
            if (DEBUG) Debug.Log("Could not resolve assetStatModifier. Asset is null");
            return null;
        }

        return assetStatModifier;
    }

    //

    public static List<DataModeledAssetStat> TranslateAssetStatModifiersToDataPersistentAssetStats(List<AssetStatModifier> assetStatModifiers)
    {
        List<DataModeledAssetStat> dataPersistentAssetStats = new List<DataModeledAssetStat>();

        foreach (AssetStatModifier assetStatModifier in assetStatModifiers)
        {
            DataModeledAssetStat dataPersistentAssetStat = TranslateAssetStatModifierToDataPersistentAssetStat(assetStatModifier);
            if (dataPersistentAssetStat == null) continue;
            dataPersistentAssetStats.Add(dataPersistentAssetStat);
        }

        return dataPersistentAssetStats;
    }

    public static DataModeledAssetStat TranslateAssetStatModifierToDataPersistentAssetStat(AssetStatModifier assetStatModifier)
    {
        string originGUID = assetStatModifier.originGUID;
        string numericStatType = assetStatModifier.assetStatType.ToString();
        string numericStatModificationType = assetStatModifier.assetStatModificationType.ToString();
        int assetID = assetStatModifier.asset.id;

        DataModeledAssetStat dataPersistentAssetStat = new DataModeledAssetStat(originGUID, numericStatType, numericStatModificationType, assetID);
        return dataPersistentAssetStat;
    }
    #endregion
}
