using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxShieldStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.MaxShield;
}
