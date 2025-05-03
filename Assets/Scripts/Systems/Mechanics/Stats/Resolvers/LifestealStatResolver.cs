using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealStatResolver : NumericStatResolver
{
    public static LifestealStatResolver Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one LifestealStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.Lifesteal;
}
