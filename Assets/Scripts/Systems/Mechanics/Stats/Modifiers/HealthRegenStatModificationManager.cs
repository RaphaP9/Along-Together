using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatModificationManager : NumericStatModificationManager
{
    public static HealthRegenStatModificationManager Instance { get; private set; }

    public static event EventHandler OnHealthRegenStatInitialized;
    public static event EventHandler OnHealthRegenStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one HealthRegenStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.HealthRegen;

    protected override void InitializeStat()
    {
        OnHealthRegenStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnHealthRegenStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int ResolveHealthRegenStat(CharacterSO characterSO)
    {
        int baseHealthRegen = characterSO.healthRegen;

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

        int accumulatedHealthRegen = baseHealthRegen;

        foreach (NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedHealthRegen += Mathf.CeilToInt(numericStatModifier.value);
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedHealthRegen += Mathf.CeilToInt(numericStatModifier.value);
        }


        float accumulatedHealthRegenMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedHealthRegenMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedHealthRegenMultiplier += numericStatModifier.value;
        }

        int resolvedHealthRegen = Mathf.CeilToInt(accumulatedHealthRegen * accumulatedHealthRegenMultiplier);

        return resolvedHealthRegen;
    }
}
