using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualNumericStatModifierManager : NumericStatModifierManager
{
    public static PerpetualNumericStatModifierManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PerpetualNumericStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        numericStatModifiers = DataUtilities.TranslateDataPersistentNumericStatsToNumericStatModifiers(SessionPerpetualDataContainer.Instance.PerpetualData.numericStats);
    }

    protected override void SaveRuntimeData()
    {
        SessionPerpetualDataContainer.Instance.PerpetualData.numericStats = DataUtilities.TranslateNumericStatModifiersToDataPersistentNumericStats(numericStatModifiers);
    }
}
