using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeChanceStatModificationManager : NumericStatModificationManager
{
    public static DodgeChanceStatModificationManager Instance { get; private set; }

    public static event EventHandler OnDodgeChanceStatInitialized;
    public static event EventHandler OnDodgeChanceStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DodgeChanceStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.DodgeChance;

    protected override void InitializeStat()
    {
        OnDodgeChanceStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnDodgeChanceStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public float ResolveDodgeChanceStat(CharacterSO characterSO)
    {
        float baseDodgeChance = characterSO.dodgeChance;

        if (temporalReplacementStatModifiers.Count > 0)
        {
            float rawValue = temporalReplacementStatModifiers[^1].value;
            return rawValue;
        }

        if (permanentReplacementStatModifiers.Count > 0)
        {
            float rawValue = permanentReplacementStatModifiers[^1].value;
            return rawValue;
        }

        float accumulatedDodgeChance = baseDodgeChance;

        foreach (NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedDodgeChance += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedDodgeChance += numericStatModifier.value;
        }


        float accumulatedDodgeChanceMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedDodgeChanceMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedDodgeChanceMultiplier += numericStatModifier.value;
        }

        float resolvedDodgeChance = GeneralUtilities.RoundToNDecimalPlaces(accumulatedDodgeChance * accumulatedDodgeChanceMultiplier, 2);

        return resolvedDodgeChance;
    }
}
