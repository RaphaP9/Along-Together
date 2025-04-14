using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetStatModificationManager<T> : StatModificationManager where T : ScriptableObject
{
    [Header("Permanent Lists - Runtime Filled")]
    [SerializeField] protected List<AssetStatModifier> permanentUnionStatModifiers;
    [SerializeField] protected List<AssetStatModifier> permanentReplacementStatModifiers;

    [Header("Temporal Lists - Runtime Filled")]
    [SerializeField] protected List<AssetStatModifier> temporalUnionStatModifiers;
    [SerializeField] protected List<AssetStatModifier> temporalReplacementStatModifiers;

    public List<AssetStatModifier> PermanentUnionStatModifiers => permanentUnionStatModifiers;
    public List<AssetStatModifier> PermanentReplacementStatModifiers => permanentReplacementStatModifiers;

    public List<AssetStatModifier> TemporalUnionStatModifiers => temporalUnionStatModifiers;
    public List<AssetStatModifier> TemporalReplacementStatModifiers => temporalReplacementStatModifiers;

    #region In-Line Methods
    public bool HasPermanentUnionStatModifiers() => permanentUnionStatModifiers.Count > 0;
    public bool HasPermanentReplacementStatModifiers() => permanentReplacementStatModifiers.Count > 0;

    public int GetPermanentUnionStatModifiersQuantity() => permanentUnionStatModifiers.Count;
    public int GetPermanentReplacementStatModifiersQuantity() => permanentReplacementStatModifiers.Count;

    public bool HasTemporalUnionStatModifiers() => temporalUnionStatModifiers.Count > 0;
    public bool HasTemporalReplacementStatModifiers() => temporalReplacementStatModifiers.Count > 0;

    public int GetTemporalUnionStatModifiersQuantity() => temporalUnionStatModifiers.Count;
    public int GetTemporalReplacementStatModifiersQuantity() => temporalReplacementStatModifiers.Count;

    public override bool HasPermanentStatModifiers() => GetPermanentStatModifiersQuantity() > 0;
    public override int GetPermanentStatModifiersQuantity() => permanentUnionStatModifiers.Count + permanentReplacementStatModifiers.Count;

    public override bool HasTemporalStatModifiers() => GetTemporalStatModifiersQuantity() > 0;
    public override int GetTemporalStatModifiersQuantity() => temporalUnionStatModifiers.Count + temporalReplacementStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Asset;

    #endregion

    #region Permanent Stat Modifiers
    public override void AddPermanentStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        foreach (AssetEmbeddedStat assetEmbeddedStat in embeddedStatsHolder.GetAssetEmbeddedStats())
        {
            AddPermanentAssetStatModifier(originGUID, assetEmbeddedStat);
        }
    }

    protected void AddPermanentAssetStatModifier(string originGUID, AssetEmbeddedStat assetEmbeddedStat)
    {
        if (assetEmbeddedStat == null)
        {
            if (debug) Debug.Log("AssetEmbeddedStat is null. StatModifier will not be added");
            return;
        }

        if (assetEmbeddedStat.GetStatValueType() != GetStatValueType()) return;
        if (assetEmbeddedStat.statType != GetStatType()) return;

        AssetStatModifier assetStatModifier = new AssetStatModifier { originGUID = originGUID, statType = assetEmbeddedStat.statType, assetStatModificationType = assetEmbeddedStat.assetStatModificationType, asset = assetEmbeddedStat.asset };

        switch (assetEmbeddedStat.assetStatModificationType)
        {
            case AssetStatModificationType.Union:
            default:
                permanentUnionStatModifiers.Add(assetStatModifier);
                break;
            case AssetStatModificationType.Replacement:
                permanentReplacementStatModifiers.Add(assetStatModifier);
                break;
        }

        UpdateStat();
    }

    public override void RemovePermanentStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be removed");
            return;
        }

        permanentUnionStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        permanentReplacementStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        UpdateStat();
    }
    #endregion

    #region Temporal Stat Modifiers
    public override void AddTemporalStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        foreach (AssetEmbeddedStat assetEmbeddedStat in embeddedStatsHolder.GetAssetEmbeddedStats())
        {
            AddTemporalAssetStatModifier(originGUID, assetEmbeddedStat);
        }
    }

    protected void AddTemporalAssetStatModifier(string originGUID, AssetEmbeddedStat assetEmbeddedStat)
    {
        if (assetEmbeddedStat == null)
        {
            if (debug) Debug.Log("AssetEmbeddedStat is null. StatModifier will not be added");
            return;
        }

        if (assetEmbeddedStat.GetStatValueType() != GetStatValueType()) return;
        if (assetEmbeddedStat.statType != GetStatType()) return;

        AssetStatModifier assetStatModifier = new AssetStatModifier { originGUID = originGUID, statType = assetEmbeddedStat.statType, assetStatModificationType = assetEmbeddedStat.assetStatModificationType, asset = assetEmbeddedStat.asset };

        switch (assetEmbeddedStat.assetStatModificationType)
        {
            case AssetStatModificationType.Union:
            default:
                temporalUnionStatModifiers.Add(assetStatModifier);
                break;
            case AssetStatModificationType.Replacement:
                temporalReplacementStatModifiers.Add(assetStatModifier);
                break;
        }

        UpdateStat();
    }

    public override void RemoveTemporalStatModifiersByGUID(string originGUID)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be removed");
            return;
        }

        temporalUnionStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        temporalReplacementStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        UpdateStat();
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
