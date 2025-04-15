using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumericStatModificationManager : StatModificationManager
{
    [Header("Permanent Lists - Runtime Filled")]
    [SerializeField] protected List<NumericStatModifier> valueStatModifiers;
    [SerializeField] protected List<NumericStatModifier> percentageStatModifiers;
    [SerializeField] protected List<NumericStatModifier> replacementStatModifiers;

    public List<NumericStatModifier> ValueStatModifiers => valueStatModifiers;
    public List<NumericStatModifier> PercentageStatModifiers => percentageStatModifiers;
    public List<NumericStatModifier> ReplacementStatModifiers => replacementStatModifiers;


    #region In-Line Methods
    public bool HasValueStatModifiers() => valueStatModifiers.Count > 0;
    public bool HasPercentageStatModifiers() => percentageStatModifiers.Count > 0;
    public bool HasReplacementStatModifiers() => replacementStatModifiers.Count > 0;

    public int GetValueStatModifiersQuantity() => valueStatModifiers.Count;
    public int GetPercentageStatModifiersQuantity() => percentageStatModifiers.Count;
    public int GetReplacementStatModifiersQuantity() => replacementStatModifiers.Count;

    public override bool HasStatModifiers() => GetStatModifiersQuantity() > 0;
    public override int GetStatModifiersQuantity() => valueStatModifiers.Count + percentageStatModifiers.Count + replacementStatModifiers.Count;

    protected override StatValueType GetStatValueType() => StatValueType.Numeric;

    #endregion

    #region Add/Remove Stat Modifiers
    public override void AddStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder)
    {
        if (originGUID == "")
        {
            if (debug) Debug.Log("GUID is empty. StatModifiers will not be added");
            return;
        }

        int statsAdded = 0;

        foreach (NumericEmbeddedStat numericEmbeddedStat in embeddedStatsHolder.GetNumericEmbeddedStats())
        {
            if (AddNumericStatModifier(originGUID, numericEmbeddedStat)) statsAdded++;
        }

        if (statsAdded > 0) UpdateStat();
    }

    protected bool AddNumericStatModifier(string originGUID, NumericEmbeddedStat numericEmbeddedStat)
    {
        if (numericEmbeddedStat == null)
        {
            if (debug) Debug.Log("NumericEmbeddedStat is null. StatModifier will not be added");
            return false;
        }

        if (numericEmbeddedStat.GetStatValueType() != GetStatValueType()) return false;
        if (numericEmbeddedStat.statType != GetStatType()) return false; 

        NumericStatModifier numericStatModifier = new NumericStatModifier {originGUID = originGUID, statType = numericEmbeddedStat.statType, numericStatModificationType = numericEmbeddedStat.numericStatModificationType, value = numericEmbeddedStat.value};
        
        switch (numericEmbeddedStat.numericStatModificationType)
        {
            case NumericStatModificationType.Value:
            default:
                valueStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Percentage:
                percentageStatModifiers.Add(numericStatModifier);
                break;
            case NumericStatModificationType.Replacement:
                replacementStatModifiers.Add(numericStatModifier);
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

        int removedFromValue = valueStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        int removedFromPercentage = percentageStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);
        int removedFromReplacement = replacementStatModifiers.RemoveAll(statModifier => statModifier.originGUID == originGUID);

        int totalRemoved = removedFromValue + removedFromPercentage + removedFromReplacement;

        if (totalRemoved > 0) UpdateStat();
    }
    #endregion
}