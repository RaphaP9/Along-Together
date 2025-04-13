using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritDamageMultiplierStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.AttackCritDamage;
}
