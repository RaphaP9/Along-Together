using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorStatModifier : NumericStatModifier
{
    public float value;

    public override StatType GetStatType() => StatType.Armor;
}
