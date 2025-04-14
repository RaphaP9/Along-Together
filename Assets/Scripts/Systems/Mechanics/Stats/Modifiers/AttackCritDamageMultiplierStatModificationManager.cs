using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatModificationManager : NumericStatModificationManager
{
    public static AttackCritDamageMultiplierStatModificationManager Instance { get; private set; }

    public static event EventHandler OnAttackCritDamageMultiplierStatInitialized;
    public static event EventHandler OnAttackCritDamageMultiplierStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritDamageMultiplierStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.AttackCritDamageMultiplier;

    protected override void InitializeStat()
    {
        OnAttackCritDamageMultiplierStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnAttackCritDamageMultiplierStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public float ResolveAttackCritDamageMultiplierStat(CharacterSO characterSO)
    {
        float baseAttackCritDamageMultiplier = characterSO.attackCritDamageMultiplier;

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

        float accumulatedAttackCritDamageMultiplier = baseAttackCritDamageMultiplier;

        foreach (NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedAttackCritDamageMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedAttackCritDamageMultiplier += numericStatModifier.value;
        }


        float accumulatedAttackCritDamageMultiplierMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedAttackCritDamageMultiplierMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedAttackCritDamageMultiplierMultiplier += numericStatModifier.value;
        }

        float resolvedAttackCritDamageMultiplier = GeneralUtilities.RoundToNDecimalPlaces(accumulatedAttackCritDamageMultiplier * accumulatedAttackCritDamageMultiplierMultiplier, 2);

        return resolvedAttackCritDamageMultiplier;
    }
}
