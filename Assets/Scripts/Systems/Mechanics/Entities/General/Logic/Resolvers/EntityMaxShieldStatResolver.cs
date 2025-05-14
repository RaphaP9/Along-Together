using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMaxShieldStatResolver : EntityIntStatResolver
{
    protected override int CalculateStat() => entitySO.baseShield;
}
