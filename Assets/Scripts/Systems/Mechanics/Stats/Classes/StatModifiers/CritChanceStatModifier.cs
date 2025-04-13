using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritChanceStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.AttackCritChance;
}