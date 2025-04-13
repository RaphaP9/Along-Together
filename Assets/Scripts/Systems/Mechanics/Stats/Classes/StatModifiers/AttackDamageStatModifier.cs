using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.AttackDamage;
}