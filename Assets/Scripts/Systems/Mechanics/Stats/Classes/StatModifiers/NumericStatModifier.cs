using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NumericStatModifier : StatModifier
{
    public NumericStatType numericStatType;
    public NumericStatModificationType numericStatModificationType;
    public float value;

    public override StatValueType GetStatValueType() => StatValueType.Numeric;
}
