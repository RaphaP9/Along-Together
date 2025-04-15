using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatResolver : StatResolver
{
    public static MaxHealthStatResolver Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<MaxHealthStatModificationManager> maxHealthStatModificationManagers;

    public List<MaxHealthStatModificationManager> MaxHealthStatModificationManagers => maxHealthStatModificationManagers;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MaxHealthStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public int ResolveMaxHealthStat(CharacterSO characterSO)
    {
        int baseMaxHealth = characterSO.healthPoints;

        foreach(MaxHealthStatModificationManager statManager in maxHealthStatModificationManagers)
        {
            if(statManager.ReplacementStatModifiers.Count > 0)
            {
                float rawValue = statManager.ReplacementStatModifiers[^1].value; //Return the first 
                return Mathf.CeilToInt(rawValue);
            }
        }

        int accumulatedMaxHealth = baseMaxHealth;
        float accumulatedMaxHealthMultiplier = 1f;

        foreach (MaxHealthStatModificationManager statManager in maxHealthStatModificationManagers)
        {
            foreach(NumericStatModifier statModifier in statManager.ValueStatModifiers)
            {
                accumulatedMaxHealth += Mathf.CeilToInt(statModifier.value);
            }
        }

        foreach (MaxHealthStatModificationManager statManager in maxHealthStatModificationManagers)
        {
            foreach (NumericStatModifier statModifier in statManager.PercentageStatModifiers)
            {
                accumulatedMaxHealthMultiplier += statModifier.value;
            }
        }

        int resolvedMaxHealth = Mathf.CeilToInt(accumulatedMaxHealth * accumulatedMaxHealthMultiplier);

        return resolvedMaxHealth;
    }
}
