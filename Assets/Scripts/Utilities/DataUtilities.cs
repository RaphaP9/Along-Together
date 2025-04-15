using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;
using UnityEngine.Windows;

public static class DataUtilities 
{
    private const bool DEBUG = true;

    #region NumericStatModifierData
    public static List<NumericStatModifier> TranslateNumericStatModifiersData(List<DataPersistentNumericStat> dataPersistentNumericStats)
    {
        List<NumericStatModifier> numericStatModifiers = new List<NumericStatModifier>();

        foreach(DataPersistentNumericStat dataPersistentNumericStat in dataPersistentNumericStats)
        {
            NumericStatModifier numericStatModifier = TranslateNumericStatModifierData(dataPersistentNumericStat);
            if (numericStatModifier == null) continue;
            numericStatModifiers.Add(numericStatModifier);  
        }

        return numericStatModifiers;
    }

    public static NumericStatModifier TranslateNumericStatModifierData(DataPersistentNumericStat dataPersistentNumericStat)
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
    #endregion

    #region AssetStatModifierData
    public static List<AssetStatModifier> TranslateAssetStatModifiersData(List<DataPersistentAssetStat> dataPersistentAssetStats)
    {
        List<AssetStatModifier> assetStatModifiers = new List<AssetStatModifier>();

        foreach (DataPersistentAssetStat dataPersistentAssetStat in dataPersistentAssetStats)
        {
            AssetStatModifier assetStatModifier = TranslateAssetStatModifierData(dataPersistentAssetStat);
            if (assetStatModifier == null) continue;
            assetStatModifiers.Add(assetStatModifier);
        }

        return assetStatModifiers;
    }

    public static AssetStatModifier TranslateAssetStatModifierData(DataPersistentAssetStat dataPersistentAssetStat)
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
    #endregion
}
