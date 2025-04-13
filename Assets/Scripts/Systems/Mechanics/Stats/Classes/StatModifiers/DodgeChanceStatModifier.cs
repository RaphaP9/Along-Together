using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeChanceStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.DodgeChance;
}