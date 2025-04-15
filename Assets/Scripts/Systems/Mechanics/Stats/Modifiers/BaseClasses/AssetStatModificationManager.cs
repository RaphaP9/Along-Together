using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetStatModificationManager<T> : StatModificationManager where T : ScriptableObject
{
    [Header("Permanent Lists - Runtime Filled")]
    [SerializeField] protected List<AssetStatModifier> unionStatModifiers;
    [SerializeField] protected List<AssetStatModifier> replacementStatModifiers;

    public List<AssetStatModifier> UnionStatModifiers => unionStatModifiers;
    public List<AssetStatModifier> ReplacementStatModifiers => replacementStatModifiers;


    #region In-Line Methods
    public bool HasUnionStatModifiers() => unionStatModifiers.Count > 0;
    public bool HasReplacementStatModifiers() => replacementStatModifiers.Count > 0;

    public int GetUnionStatModifiersQuantity() => unionStatModifiers.Count;
    public int GetReplacementStatModifiersQuantity() => replacementStatModifiers.Count;

    public override bool HasStatModifiers() => GetStatModifiersQuantity() > 0;
    public override int GetStatModifiersQuantity() => unionStatModifiers.Count + replacementStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Asset;

    #endregion

    #region Permanent Stat Modifiers
    public override void AddStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        int statsAdded = 0;

        foreach (AssetEmbeddedStat assetEmbeddedStat in embeddedStatsHolder.GetAssetEmbeddedStats())
        {
            if (AddAssetStatModifier(originGUID, assetEmbeddedStat)) statsAdded++;
        }

        if (statsAdded > 0) UpdateStat();
    }

    protected bool AddAssetStatModifier(string originGUID, AssetEmbeddedStat assetEmbeddedStat)
    {
        if (assetEmbeddedStat == null)
        {
            if (debug) Debug.Log("AssetEmbeddedStat is null. StatModifier will not be added");
            return false;
        }

        if (assetEmbeddedStat.GetStatValueType() != GetStatValueType()) return false;
        if (assetEmbeddedStat.statType != GetStatType()) return false;

        AssetStatModifier assetStatModifier = new AssetStatModifier { originGUID = originGUID, statType = assetEmbeddedStat.statType, assetStatModificationType = assetEmbeddedStat.assetStatModificationType, asset = assetEmbeddedStat.asset };

        switch (assetEmbeddedStat.assetStatModificationType)
        {
            case AssetStatModificationType.Union:
            default:
                unionStatModifiers.Add(assetStatModifier);
                break;
            case AssetStatModificationType.Replacement:
                replacementStatModifiers.Add(assetStatModifier);
                break;
        }

        return true;
    }

    public override void RemoveStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be removed");
            return;
        }

        int removedFromValue = unionStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        int removedFromReplacement = replacementStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        int totalRemoved = removedFromValue + removedFromReplacement;

        if (totalRemoved > 0) UpdateStat();
    }
    #endregion

    protected List<T> GetSpecificListFromGenericList(List<ScriptableObject> assetList)
    {
        List<T> specificList = new List<T>();

        foreach (ScriptableObject asset in assetList)
        {
            specificList.Add(GetSpecificFromGeneric(asset));
        }

        return specificList;
    }

    protected T GetSpecificFromGeneric(ScriptableObject asset)
    {
        return asset as T;
    }
}
