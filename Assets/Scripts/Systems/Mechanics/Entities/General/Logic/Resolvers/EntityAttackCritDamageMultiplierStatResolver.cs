using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackCritDamageMultiplierStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entitySO.baseAttackCritDamageMultiplier;
}


