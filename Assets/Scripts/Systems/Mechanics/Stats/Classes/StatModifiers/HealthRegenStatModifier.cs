using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.HealthRegen;
}