using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxShieldStatResolver : StatResolver
{
    public static MaxShieldStatResolver Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<MaxShieldStatModificationManager> maxShieldStatModificationManagers;

    public List<MaxShieldStatModificationManager> MaxShieldStatModificationManagers => maxShieldStatModificationManagers;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxShieldStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public int ResolveMaxShieldStat(CharacterSO characterSO)
    {
        int baseMaxShield = characterSO.shieldPoints;

        foreach (MaxShieldStatModificationManager statManager in maxShieldStatModificationManagers)
        {
            if (statManager.ReplacementStatModifiers.Count > 0)
            {
                float rawValue = statManager.ReplacementStatModifiers[^1].value; //Return the first 
                return Mathf.CeilToInt(rawValue);
            }
        }

        int accumulatedMaxShield = baseMaxShield;
        float accumulatedMaxShieldMultiplier = 1f;

        foreach (MaxShieldStatModificationManager statManager in maxShieldStatModificationManagers)
        {
            foreach (NumericStatModifier statModifier in statManager.ValueStatModifiers)
            {
                accumulatedMaxShield += Mathf.CeilToInt(statModifier.value);
            }
        }

        foreach (MaxShieldStatModificationManager statManager in maxShieldStatModificationManagers)
        {
            foreach (NumericStatModifier statModifier in statManager.PercentageStatModifiers)
            {
                accumulatedMaxShieldMultiplier += statModifier.value;
            }
        }

        int resolvedMaxShield = Mathf.CeilToInt(accumulatedMaxShield * accumulatedMaxShieldMultiplier);

        return resolvedMaxShield;
    }
}

