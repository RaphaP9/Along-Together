using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementSpeedStatResolver : EntityFloatStatResolver
{
    protected override float CalculateStat() => entityIdentifier.EntitySO.baseMovementSpeed;
}

