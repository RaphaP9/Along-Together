using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatModificationManager : NumericStatModificationManager
{
    public static AttackCritChanceStatModificationManager Instance { get; private set; }

    public static event EventHandler OnAttackCritChanceStatInitialized;
    public static event EventHandler OnAttackCritChanceStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackCritChanceStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.AttackCritChance;

    protected override void InitializeStat()
    {
        OnAttackCritChanceStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnAttackCritChanceStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public float ResolveAttackCritChanceStat(CharacterSO characterSO)
    {
        float baseAttackCritChance = characterSO.attackCritChance;

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

        float accumulatedAttackCritChance = baseAttackCritChance;

        foreach (NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedAttackCritChance += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedAttackCritChance += numericStatModifier.value;
        }


        float accumulatedAttackCritChanceMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedAttackCritChanceMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedAttackCritChanceMultiplier += numericStatModifier.value;
        }

        float resolvedAttackCritChance = GeneralUtilities.RoundToNDecimalPlaces(accumulatedAttackCritChance * accumulatedAttackCritChanceMultiplier, 2);

        return resolvedAttackCritChance;
    }
}