using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumericStatModificationManager : StatModificationManager
{
    [Header("Permanent Lists")]
    [SerializeField] protected List<NumericStatModifier> permanentValueStatModifiers;
    [SerializeField] protected List<NumericStatModifier> permanentPercentageStatModifiers;
    [SerializeField] protected List<NumericStatModifier> permanentReplacementStatModifiers;

    [Header("Temporal Lists")]
    [SerializeField] protected List<NumericStatModifier> temporalValueStatModifiers;
    [SerializeField] protected List<NumericStatModifier> temporalPercentageStatModifiers;
    [SerializeField] protected List<NumericStatModifier> temporalReplacementStatModifiers;

    public List<NumericStatModifier> PermanentValueStatModifiers => permanentValueStatModifiers;
    public List<NumericStatModifier> PermanentPercentageStatModifiers => permanentPercentageStatModifiers;
    public List<NumericStatModifier> PermanentReplacementStatModifiers => permanentReplacementStatModifiers;

    public List<NumericStatModifier> TemporalValueStatModifiers => temporalValueStatModifiers;
    public List<NumericStatModifier> TemporalPercentageStatModifiers => temporalPercentageStatModifiers;
    public List<NumericStatModifier> TemporalReplacementStatModifiers => temporalReplacementStatModifiers; 

    #region In-Line Methods
    public bool HasPermanentValueStatModifiers() => permanentValueStatModifiers.Count > 0;
    public bool HasPermanentPercentageStatModifiers() => permanentPercentageStatModifiers.Count > 0;
    public bool HasPermanentReplacementStatModifiers() => permanentReplacementStatModifiers.Count > 0;

    public int GetPermanentValueStatModifiersQuantity() => permanentValueStatModifiers.Count;
    public int GetPermanentPercentageStatModifiersQuantity() => permanentPercentageStatModifiers.Count;
    public int GetPermanentReplacementStatModifiersQuantity() => permanentReplacementStatModifiers.Count;

    public bool HasTemporalValueStatModifiers() => temporalValueStatModifiers.Count > 0;
    public bool HasTemporalPercentageStatModifiers() => temporalPercentageStatModifiers.Count > 0;
    public bool HasTemporalReplacementStatModifiers() => temporalReplacementStatModifiers.Count > 0;

    public int GetTemporalValueStatModifiersQuantity() => temporalValueStatModifiers.Count;
    public int GetTemporalPercentageStatModifiersQuantity() => temporalPercentageStatModifiers.Count;
    public int GetTemporalReplacementStatModifiersQuantity() => temporalReplacementStatModifiers.Count;

    public override bool HasPermanentStatModifiers() => GetPermanentStatModifiersQuantity() > 0;
    public override int GetPermanentStatModifiersQuantity() => permanentValueStatModifiers.Count + permanentPercentageStatModifiers.Count + permanentReplacementStatModifiers.Count;

    public override bool HasTemporalStatModifiers() => GetTemporalStatModifiersQuantity() > 0;
    public override int GetTemporalStatModifiersQuantity() => temporalValueStatModifiers.Count + temporalPercentageStatModifiers.Count + temporalReplacementStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Numeric;

    #endregion

    #region Permanent Stat Modifiers
    public override void AddPermanentStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        foreach (NumericEmbeddedStat numericEmbeddedStat in embeddedStatsHolder.GetNumericEmbeddedStats())
        {
            AddPermanentNumericStatModifier(originGUID, numericEmbeddedStat);
        }
    }

    protected void AddPermanentNumericStatModifier(string originGUID, NumericEmbeddedStat numericEmbeddedStat)
    {
        if (numericEmbeddedStat == null)
        {
            if (debug) Debug.Log("NumericEmbeddedStat is null. StatModifier will not be added");
            return;
        }

        if (numericEmbeddedStat.GetStatValueType() != GetStatValueType()) return;
        if (numericEmbeddedStat.statType != GetStatType()) return; 

        NumericStatModifier numericStatModifier = new NumericStatModifier {originGUID = originGUID, statType = numericEmbeddedStat.statType, numericStatModificationType = numericEmbeddedStat.numericStatModificationType, value = numericEmbeddedStat.value};
        
        switch (numericEmbeddedStat.numericStatModificationType)
        {
            case NumericStatModificationType.Value:
            default:
                permanentValueStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Percentage:
                permanentPercentageStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Replacement:
                permanentReplacementStatModifiers.Add(numericStatModifier);
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

        permanentValueStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        permanentPercentageStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
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

        foreach (NumericEmbeddedStat numericEmbeddedStat in embeddedStatsHolder.GetNumericEmbeddedStats())
        {
            AddTemporalNumericStatModifier(originGUID, numericEmbeddedStat);
        }
    }

    protected void AddTemporalNumericStatModifier(string originGUID, NumericEmbeddedStat numericEmbeddedStat)
    {
        if (numericEmbeddedStat == null)
        {
            if (debug) Debug.Log("NumericEmbeddedStat is null. StatModifier will not be added");
            return;
        }

        if (numericEmbeddedStat.GetStatValueType() != GetStatValueType()) return;
        if (numericEmbeddedStat.statType != GetStatType()) return;

        NumericStatModifier numericStatModifier = new NumericStatModifier { originGUID = originGUID, statType = numericEmbeddedStat.statType, numericStatModificationType = numericEmbeddedStat.numericStatModificationType, value = numericEmbeddedStat.value };

        switch (numericEmbeddedStat.numericStatModificationType)
        {
            case NumericStatModificationType.Value:
            default:
                temporalValueStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Percentage:
                temporalPercentageStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Replacement:
                temporalReplacementStatModifiers.Add(numericStatModifier);
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

        temporalValueStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        temporalPercentageStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        temporalReplacementStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        UpdateStat();
    }
    #endregion
}