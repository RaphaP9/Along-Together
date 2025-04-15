using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatResolver : NumericStatResolver
{
    public static MaxHealthStatResolver Instance { get; private set; }

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

    protected override NumericStatType GetNumericStatType() => NumericStatType.MaxHealth;
}
