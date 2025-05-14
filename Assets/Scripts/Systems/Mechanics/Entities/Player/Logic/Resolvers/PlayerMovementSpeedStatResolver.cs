using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSpeedStatResolver : EntityMovementSpeedStatResolver
{
    private CharacterIdentifier CharacterIdentifier => entityIdentifier as CharacterIdentifier;

    protected virtual void OnEnable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated += MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated -= MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    protected override float CalculateStat()
    {
        return MovementSpeedStatResolver.Instance.ResolveStatFloat(CharacterIdentifier.CharacterSO.baseMovementSpeed);
    }

    private void MovementSpeedStatResolver_OnMovementSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateStat();
    }
}