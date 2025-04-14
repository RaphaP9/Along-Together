using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmorStatModificationManager : NumericStatModificationManager
{
    public static ArmorStatModificationManager Instance { get; private set; }

    public static event EventHandler OnArmorStatInitialized;
    public static event EventHandler OnArmorStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ArmorStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.Armor;

    protected override void InitializeStat()
    {
        OnArmorStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnArmorStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int ResolveArmorStat(CharacterSO characterSO)
    {
        int baseArmor = characterSO.armorPoints;

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

        int accumulatedArmor = baseArmor;

        foreach(NumericStatModifier numericStatModifier in permanentValueStatModifiers)
        {
            accumulatedArmor += Mathf.CeilToInt(numericStatModifier.value);
        }

        foreach (NumericStatModifier numericStatModifier in temporalValueStatModifiers)
        {
            accumulatedArmor += Mathf.CeilToInt(numericStatModifier.value);
        }


        float accumulatedArmorMultiplier = 1f;

        foreach (NumericStatModifier numericStatModifier in permanentPercentageStatModifiers)
        {
            accumulatedArmorMultiplier += numericStatModifier.value;
        }

        foreach (NumericStatModifier numericStatModifier in temporalPercentageStatModifiers)
        {
            accumulatedArmorMultiplier += numericStatModifier.value;
        }

        int resolvedArmor = Mathf.CeilToInt(accumulatedArmor * accumulatedArmorMultiplier);

        return resolvedArmor;
    }
}