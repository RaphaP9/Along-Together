using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.MaxHealth;
}
