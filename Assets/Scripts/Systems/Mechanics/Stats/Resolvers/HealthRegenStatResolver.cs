using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatResolver : NumericStatResolver
{
    public static HealthRegenStatResolver Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one HealthRegenStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.HealthRegen;
}

