using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaxShieldStatModificationManager : NumericStatModificationManager
{
    public static MaxShieldStatModificationManager Instance { get; private set; }

    public static event EventHandler OnMaxShieldStatInitialized;
    public static event EventHandler OnMaxShieldStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxShieldStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.MaxShield;

    protected override void InitializeStat()
    {
        OnMaxShieldStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMaxShieldStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int ResolveMaxShieldStat(CharacterSO characterSO)
    {
        int baseMaxShield = characterSO.shieldPoints;

        if(temporalReplacementStatModifiers.Count > 0)
        {
            float rawValue = temporalReplacementStatModifiers[^1].value;
            return Mathf.CeilToInt(rawValue);
        }

        if (permanentReplacementStatModifiers.Count > 0)
        {
            float rawValue = permanentReplacementStatModifiers[^1].value;
            return Mathf.CeilToInt(rawValue);
        }

        int accumulatedMaxShield = baseMaxShield;

        foreach(NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedMaxShield += Mathf.CeilToInt(numericStatModifier.value);
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedMaxShield += Mathf.CeilToInt(numericStatModifier.value);
        }


        float accumulatedMaxShieldMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedMaxShieldMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedMaxShieldMultiplier += numericStatModifier.value;
        }

        int resolvedMaxShield = Mathf.CeilToInt(accumulatedMaxShield * accumulatedMaxShieldMultiplier);

        return resolvedMaxShield;
    }
}