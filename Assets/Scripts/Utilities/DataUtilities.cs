using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataUtilities 
{
    private const bool DEBUG = true;

    #region CharacterSO Translation
    public static CharacterSO TranslateCharacterIDToCharacterSO(int characterID)
    {
        if(CharacterAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("CharacterAssetLibrary is null. Can not resolve CharacterSO Asset");
            return null;
        }

        CharacterSO characterSO = CharacterAssetLibrary.Instance.GetCharacterSOByID(characterID);
        return characterSO;
    }
    #endregion

    #region Numeric Stat Translation
    public static List<NumericStatModifier> TranslateDataModeledNumericStatsToNumericStatModifiers(List<DataModeledNumericStat> dataModeledNumericStats)
    {
        List<NumericStatModifier> numericStatModifiers = new List<NumericStatModifier>();

        foreach(DataModeledNumericStat dataModeledNumericStat in dataModeledNumericStats)
        {
            NumericStatModifier numericStatModifier = TranslateDataModeledNumericStatToNumericStatModifier(dataModeledNumericStat);
            if (numericStatModifier == null) continue;
            numericStatModifiers.Add(numericStatModifier);  
        }

        return numericStatModifiers;
    }
    public static NumericStatModifier TranslateDataModeledNumericStatToNumericStatModifier(DataModeledNumericStat dataModeledNumericStat)
    {
        NumericStatModifier numericStatModifier = new NumericStatModifier();

        numericStatModifier.originGUID = dataModeledNumericStat.originGUID;

        if (Enum.TryParse<NumericStatType>(dataModeledNumericStat.numericStatType, true, out var numericStatType)) numericStatModifier.numericStatType = numericStatType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledNumericStat.numericStatType}");
            return null;
        }

        if (Enum.TryParse<NumericStatModificationType>(dataModeledNumericStat.numericStatModificationType, true, out var numericStatModificationType)) numericStatModifier.numericStatModificationType = numericStatModificationType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledNumericStat.numericStatModificationType}");
            return null;
        }

        numericStatModifier.value = dataModeledNumericStat.value;

        return numericStatModifier;
    }

    //

    public static List<DataModeledNumericStat> TranslateNumericStatModifiersToDataModeledNumericStats(List<NumericStatModifier> numericStatModifiers)
    {
        List<DataModeledNumericStat> dataModeledNumericStats = new List<DataModeledNumericStat>();

        foreach(NumericStatModifier numericStatModifier in numericStatModifiers)
        {
            DataModeledNumericStat dataModeledNumericStat = TranslateNumericStatModifierToDataModeledNumericStat(numericStatModifier);
            if (dataModeledNumericStat == null) continue;
            dataModeledNumericStats.Add(dataModeledNumericStat);
        }

        return dataModeledNumericStats;
    }

    public static DataModeledNumericStat TranslateNumericStatModifierToDataModeledNumericStat(NumericStatModifier numericStatModifier)
    {
        string originGUID = numericStatModifier.originGUID;
        string numericStatType = numericStatModifier.numericStatType.ToString();
        string numericStatModificationType = numericStatModifier.numericStatModificationType.ToString();
        float value = numericStatModifier.value;    

        DataModeledNumericStat dataModeledNumericStat = new DataModeledNumericStat(originGUID,numericStatType,numericStatModificationType,value);
        return dataModeledNumericStat;
    }

    #endregion

    #region Asset Stat Translation
    public static List<AssetStatModifier> TranslateDataModeledAssetStatsToAssetStatModifiers(List<DataModeledAssetStat> dataModeledAssetStats)
    {
        List<AssetStatModifier> assetStatModifiers = new List<AssetStatModifier>();

        foreach (DataModeledAssetStat dataModeledAssetStat in dataModeledAssetStats)
        {
            AssetStatModifier assetStatModifier = TranslateDataModeledAssetStatToAssetStatModifier(dataModeledAssetStat);
            if (assetStatModifier == null) continue;
            assetStatModifiers.Add(assetStatModifier);
        }

        return assetStatModifiers;
    }
    public static AssetStatModifier TranslateDataModeledAssetStatToAssetStatModifier(DataModeledAssetStat dataModeledAssetStat)
    {
        AssetStatModifier assetStatModifier = new AssetStatModifier();

        assetStatModifier.originGUID = dataModeledAssetStat.originGUID;

        if (Enum.TryParse<AssetStatType>(dataModeledAssetStat.assetStatType, true, out var assetStatType)) assetStatModifier.assetStatType = assetStatType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledAssetStat.assetStatType}");
            return null;
        }

        if (Enum.TryParse<AssetStatModificationType>(dataModeledAssetStat.assetStatModificationType, true, out var assetStatModificationType)) assetStatModifier.assetStatModificationType = assetStatModificationType;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledAssetStat.assetStatModificationType}");
            return null;
        }

        //Now we will translate the ID according to the AssetStatType

        switch (assetStatModifier.assetStatType)
        {
            case AssetStatType.MovementType:
            default:
                /* EXAMPLE:
                #region MovementType Translation
                if (MovementTypeAssetLibrary.Instance == null)
                {
                    if (DEBUG) Debug.Log("MovementTypeAssetLibrary is null. Can not resolve MovementTypeSO Asset");
                    return null;
                }
                assetStatModifier.asset = MovementTypeAssetLibrary.Instance.GetMovementTypeSOByID(dataModeledAssetStat.assetID);
                #endregion
                */
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

    public static List<DataModeledAssetStat> TranslateAssetStatModifiersToDataModeledAssetStats(List<AssetStatModifier> assetStatModifiers)
    {
        List<DataModeledAssetStat> dataModeledAssetStats = new List<DataModeledAssetStat>();

        foreach (AssetStatModifier assetStatModifier in assetStatModifiers)
        {
            DataModeledAssetStat dataModeledAssetStat = TranslateAssetStatModifierToDataModeledAssetStat(assetStatModifier);
            if (dataModeledAssetStat == null) continue;
            dataModeledAssetStats.Add(dataModeledAssetStat);
        }

        return dataModeledAssetStats;
    }

    public static DataModeledAssetStat TranslateAssetStatModifierToDataModeledAssetStat(AssetStatModifier assetStatModifier)
    {
        string originGUID = assetStatModifier.originGUID;
        string numericStatType = assetStatModifier.assetStatType.ToString();
        string numericStatModificationType = assetStatModifier.assetStatModificationType.ToString();
        int assetID = assetStatModifier.asset.id;

        DataModeledAssetStat dataModeledAssetStat = new DataModeledAssetStat(originGUID, numericStatType, numericStatModificationType, assetID);
        return dataModeledAssetStat;
    }
    #endregion

    #region Ability Level Group Translation

    public static List<PrimitiveAbilityLevelGroup> TranslateDataModeledAbilityLevelGroupsToPrimitiveAbilityLevelGroups(List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups)
    {
        List<PrimitiveAbilityLevelGroup> primitiveAbilityLevelGroups = new List<PrimitiveAbilityLevelGroup>();

        foreach (DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup in dataModeledAbilityLevelGroups)
        {
            PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup = TranslateDataModeledAbilityLevelGroupToPrimitiveAbilityLevelGroup(dataModeledAbilityLevelGroup);
            if (primitiveAbilityLevelGroups == null) continue;
            primitiveAbilityLevelGroups.Add(primitiveAbilityLevelGroup);
        }

        return primitiveAbilityLevelGroups;
    }

    private static PrimitiveAbilityLevelGroup TranslateDataModeledAbilityLevelGroupToPrimitiveAbilityLevelGroup(DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup)
    {
        PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup = new PrimitiveAbilityLevelGroup();

        if (Enum.TryParse<AbilityLevel>(dataModeledAbilityLevelGroup.abilityLevel, true, out var abilityLevel)) primitiveAbilityLevelGroup.abilityLevel = abilityLevel;
        else
        {
            if (DEBUG) Debug.Log($"Can not resolve enum from string:{dataModeledAbilityLevelGroup.abilityLevel}");
            return null;
        }

        if (AbilitiesAssetLibrary.Instance == null)
        {
            if (DEBUG) Debug.Log("AbilitiesAssetLibrary is null. Can not resolve AbilitySO Asset.");
            return null;
        }

        primitiveAbilityLevelGroup.abilitySO = AbilitiesAssetLibrary.Instance.GetAbilitySOByID(dataModeledAbilityLevelGroup.abilityID);

        return primitiveAbilityLevelGroup;
    }

    public static List<DataModeledAbilityLevelGroup> TranslatePrimitiveAbilityLevelGroupsToDataModeledAbilityLevelGroups(List<PrimitiveAbilityLevelGroup> primitiveAbilityLevelGroups)
    {
        List<DataModeledAbilityLevelGroup> dataModeledAbilityLevelGroups = new List<DataModeledAbilityLevelGroup>();

        foreach (PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup in primitiveAbilityLevelGroups)
        {
            DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup = TranslatePrimitiveAbilityLevelGroupToDataModeledAbilityLevelGroup(primitiveAbilityLevelGroup);
            if (dataModeledAbilityLevelGroup == null) continue;
            dataModeledAbilityLevelGroups.Add(dataModeledAbilityLevelGroup);
        }

        return dataModeledAbilityLevelGroups;
    }

    public static DataModeledAbilityLevelGroup TranslatePrimitiveAbilityLevelGroupToDataModeledAbilityLevelGroup(PrimitiveAbilityLevelGroup primitiveAbilityLevelGroup)
    {
        int abilityID = primitiveAbilityLevelGroup.abilitySO.id;
        string abilityLevel = primitiveAbilityLevelGroup.abilityLevel.ToString();

        DataModeledAbilityLevelGroup dataModeledAbilityLevelGroup = new DataModeledAbilityLevelGroup { abilityID = abilityID, abilityLevel = abilityLevel };

        return dataModeledAbilityLevelGroup;
    }

    #endregion
}
