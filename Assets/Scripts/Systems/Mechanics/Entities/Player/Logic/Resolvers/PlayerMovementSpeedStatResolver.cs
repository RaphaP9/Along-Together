using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSpeedStatResolver : EntityMovementSpeedStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated += MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
        entitySlowStatusEffectHandler.OnSlowStatusEffectValueRecauculated += EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated;
    }

    protected virtual void OnDisable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated -= MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
        entitySlowStatusEffectHandler.OnSlowStatusEffectValueRecauculated -= EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated;
    }

    protected override float CalculateStat()
    {
        float resolvedValue = MovementSpeedStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseMovementSpeed) * (1 - entitySlowStatusEffectHandler.SlowPercentageResolvedValue);
        return resolvedValue;
    }

    private void MovementSpeedStatResolver_OnMovementSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }

    private void EntitySlowStatusEffectHandler_OnSlowStatusEffectValueRecauculated(object sender, EntitySlowStatusEffectHandler.OnSlowStatusEffectValueEventArgs e)
    {
        RecalculateStat(); //Leave this line to trigger OnEntityStatUpdated event (HUD values update)
    }
}