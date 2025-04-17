using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNumericStatModifierManager : NumericStatModifierManager
{
    public static RunNumericStatModifierManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one RunNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        numericStatModifiers = DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionRunDataContainer.Instance.RunData.numericStats);
    }
    protected override void SaveRuntimeData()
    {
        SessionRunDataContainer.Instance.RunData.numericStats = DataUtilities.TranslateNumericStatModifiersToDataPersistentNumericStats(numericStatModifiers);
    }
}
