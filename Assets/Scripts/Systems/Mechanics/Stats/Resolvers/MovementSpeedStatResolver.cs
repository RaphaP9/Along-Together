using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedStatResolver : NumericStatResolver
{
    public static MovementSpeedStatResolver Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementSpeedStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.MovementSpeed;
}
