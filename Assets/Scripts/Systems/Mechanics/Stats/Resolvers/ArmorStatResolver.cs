using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStatResolver : StatResolver
{
    public static ArmorStatResolver Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<ArmorStatModificationManager> armorStatModificationManagers;

    public List<ArmorStatModificationManager> ArmorStatModificationManagers => armorStatModificationManagers;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ArmorStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public int ResolveArmorStat(CharacterSO characterSO)
    {
        int baseArmor = characterSO.armorPoints;

        foreach (ArmorStatModificationManager statManager in armorStatModificationManagers)
        {
            if (statManager.ReplacementStatModifiers.Count > 0)
            {
                float rawValue = statManager.ReplacementStatModifiers[^1].value; //Return the first 
                return Mathf.CeilToInt(rawValue);
            }
        }

        int accumulatedArmor = baseArmor;
        float accumulatedArmorMultiplier = 1f;

        foreach (ArmorStatModificationManager statManager in armorStatModificationManagers)
        {
            foreach (NumericStatModifier statModifier in statManager.ValueStatModifiers)
            {
                accumulatedArmor += Mathf.CeilToInt(statModifier.value);
            }
        }

        foreach (ArmorStatModificationManager statManager in armorStatModificationManagers)
        {
            foreach (NumericStatModifier statModifier in statManager.PercentageStatModifiers)
            {
                accumulatedArmorMultiplier += statModifier.value;
            }
        }

        int resolvedArmor = Mathf.CeilToInt(accumulatedArmor * accumulatedArmorMultiplier);

        return resolvedArmor;
    }
}
