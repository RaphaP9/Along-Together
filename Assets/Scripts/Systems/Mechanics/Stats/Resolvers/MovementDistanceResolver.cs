using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDistanceResolver : NumericStatResolver
{
    public static MovementDistanceResolver Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementDistanceResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override NumericStatType GetNumericStatType() => NumericStatType.MovementDistance;
}
