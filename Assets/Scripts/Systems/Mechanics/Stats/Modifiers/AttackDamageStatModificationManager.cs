using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatModificationManager : NumericStatModificationManager
{
    public static AttackDamageStatModificationManager Instance { get; private set; }

    public static event EventHandler OnAttackDamageStatInitialized;
    public static event EventHandler OnAttackDamageStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one AttackDamageStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.AttackDamage;

    protected override void InitializeStat()
    {
        OnAttackDamageStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnAttackDamageStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int ResolveAttackDamageStat(CharacterSO characterSO)
    {
        int baseAttackDamage = characterSO.attackDamage;

        if (temporalReplacementStatModifiers.Count > 0)
        {
            float rawValue = temporalReplacementStatModifiers[^1].value;
            return Mathf.CeilToInt(rawValue);
        }

        if (permanentReplacementStatModifiers.Count > 0)
        {
            float rawValue = permanentReplacementStatModifiers[^1].value;
            return Mathf.CeilToInt(rawValue);
        }

        int accumulatedAttackDamage = baseAttackDamage;

        foreach (NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedAttackDamage += Mathf.CeilToInt(numericStatModifier.value);
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedAttackDamage += Mathf.CeilToInt(numericStatModifier.value);
        }


        float accumulatedAttackDamageMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedAttackDamageMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedAttackDamageMultiplier += numericStatModifier.value;
        }

        int resolvedAttackDamage = Mathf.CeilToInt(accumulatedAttackDamage * accumulatedAttackDamageMultiplier);

        return resolvedAttackDamage;
    }
}
