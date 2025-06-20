using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementSpeedStatResolver : EntityFloatStatResolver
{
    [Header("Entity Components")]
    [SerializeField] protected EntitySlowStatusEffectHandler entitySlowStatusEffectHandler;

    protected override float CalculateStat() => entityIdentifier.EntitySO.baseMovementSpeed * (1-entitySlowStatusEffectHandler.SlowPercentageResolvedValue); 
    protected override float CalculateBaseValue() => entityIdentifier.EntitySO.baseMovementSpeed;
}

