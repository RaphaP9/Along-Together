using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineFreeLook;
using UnityEngine.Windows;

public static class DataUtilities 
{
    public static List<NumericStatModifier> TranslateNumericStatModifiersData(List<DataPersistentNumericStat> dataPersistentNumericStats)
    {
        List<NumericStatModifier> numericStatModifiers = new List<NumericStatModifier>();

        foreach(DataPersistentNumericStat dataPersistentNumericStat in dataPersistentNumericStats)
        {
            NumericStatModifier numericStatModifier = TranslateNumericStatModifierData(dataPersistentNumericStat);
            if (numericStatModifier == null) continue;
            numericStatModifiers.Add(numericStatModifier);  
        }

        return numericStatModifiers;
    }

    public static NumericStatModifier TranslateNumericStatModifierData(DataPersistentNumericStat dataPersistentNumericStat)
    {
        NumericStatModifier numericStatModifier = new NumericStatModifier();

        numericStatModifier.originGUID = dataPersistentNumericStat.originGUID;

        if (Enum.TryParse<NumericStatType>(dataPersistentNumericStat.numericStatType, true, out var numericStatType)) numericStatModifier.numericStatType = numericStatType;
        else return null;

        if (Enum.TryParse<NumericStatModificationType>(dataPersistentNumericStat.numericStatModificationType, true, out var numericStatModificationType)) numericStatModifier.numericStatModificationType = numericStatModificationType;
        else return null;

        numericStatModifier.value = dataPersistentNumericStat.value;

        return numericStatModifier;
    }
}
