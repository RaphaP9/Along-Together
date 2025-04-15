using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NumericStatResolver : StatResolver
{
    [Header("Lists")]
    [SerializeField] private List<NumericStatModifierManager> numericStatModifierManagers;

    public List<NumericStatModifierManager> NumericStatModifierManagers => numericStatModifierManagers;

    public virtual float ResolveStatFloat(float baseValue)
    {
        float accumulatedStatValue = baseValue;
        float accumulatedStatPercentage = 1f;

        foreach (NumericStatModifierManager numericStatModifierManager in numericStatModifierManagers)
        {
            foreach (NumericStatModifier numericStatModifier in numericStatModifierManager.NumericStatModifiers)
            {
                if (numericStatModifier.numericStatType != GetNumericStatType()) continue;

                switch (numericStatModifier.numericStatModificationType)
                {
                    case NumericStatModificationType.Value:
                    default:
                        accumulatedStatValue += numericStatModifier.value;
                        break;
                    case NumericStatModificationType.Percentage:
                        accumulatedStatPercentage += numericStatModifier.value;
                        break;
                    case NumericStatModificationType.Replacement:
                        return numericStatModifier.value;                      
                }
            }
        }

        float resolvedValue = accumulatedStatValue * accumulatedStatPercentage;
        return resolvedValue;
    }

    public virtual int ResolveStatInt(int baseValue)
    {
        float resolvedValue = ResolveStatFloat(baseValue);
        int resolvedValueInt = Mathf.CeilToInt(resolvedValue);
        return resolvedValueInt;
    }

    protected abstract NumericStatType GetNumericStatType();
}
