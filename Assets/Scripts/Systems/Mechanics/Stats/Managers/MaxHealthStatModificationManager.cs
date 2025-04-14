using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatModificationManager : NumericStatModificationManager
{
    public static MaxHealthStatModificationManager Instance { get; private set; }

    public static event EventHandler OnMaxHealthStatInitialized;
    public static event EventHandler OnMaxHealthStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxHealthStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.MaxHealth;

    protected override void InitializeStat()
    {
        OnMaxHealthStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMaxHealthStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int ResolveMaxHealthStat(CharacterSO characterSO)
    {
        int baseMaxHealth = characterSO.healthPoints;

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

        int accumulatedMaxHealth = baseMaxHealth;

        foreach(NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedMaxHealth += Mathf.CeilToInt(numericStatModifier.value);
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedMaxHealth += Mathf.CeilToInt(numericStatModifier.value);
        }


        float accumulatedMaxHealthMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedMaxHealthMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedMaxHealthMultiplier += numericStatModifier.value;
        }

        int resolvedMaxHealth = Mathf.CeilToInt(accumulatedMaxHealth * accumulatedMaxHealthMultiplier);

        return resolvedMaxHealth;
    }
}
