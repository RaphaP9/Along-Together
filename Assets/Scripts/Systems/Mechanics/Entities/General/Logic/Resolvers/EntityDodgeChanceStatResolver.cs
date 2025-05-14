using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDodgeChanceStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entitySO.baseDodgeChance;
}

