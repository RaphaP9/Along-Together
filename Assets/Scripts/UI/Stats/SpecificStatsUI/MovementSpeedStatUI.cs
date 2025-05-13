using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSpeedStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerMovementSpeedChanged += SpecificPlayerStatsResolver_OnPlayerMovementSpeedChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerMovementSpeedChanged -= SpecificPlayerStatsResolver_OnPlayerMovementSpeedChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleFloat(currentValue, 2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseMovementSpeed;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.MovementSpeed;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerMovementSpeedChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}