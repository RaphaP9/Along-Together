using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedStatUI : NumericStatUI
{
    private void OnEnable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverInitialized += MovementSpeedStatResolver_OnMovementSpeedResolverInitialized;
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated += MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    private void OnDisable()
    {
        MovementSpeedStatResolver.OnMovementSpeedResolverInitialized -= MovementSpeedStatResolver_OnMovementSpeedResolverInitialized;
        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated -= MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseMovementSpeed;
    protected override float GetCurrentValue() => MovementSpeedStatResolver.Instance.ResolveStatFloat(PlayerCharacterManager.Instance.CharacterSO.baseMovementSpeed);


    #region Subscriptions
    private void MovementSpeedStatResolver_OnMovementSpeedResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void MovementSpeedStatResolver_OnMovementSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}