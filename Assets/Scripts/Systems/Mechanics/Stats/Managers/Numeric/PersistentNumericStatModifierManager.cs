using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentNumericStatModifierManager : NumericStatModifierManager
{
    public static PersistentNumericStatModifierManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PersistentNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        numericStatModifiers = DataUtilities.TranslateNumericStatModifiersData(RuntimeGameData.runtimeNumericStats);
    }
}
