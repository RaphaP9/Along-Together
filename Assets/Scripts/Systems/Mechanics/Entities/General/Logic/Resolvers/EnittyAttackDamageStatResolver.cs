using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnittyAttackDamageStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entityIdentifier.EntitySO.baseAttackDamage;
}

