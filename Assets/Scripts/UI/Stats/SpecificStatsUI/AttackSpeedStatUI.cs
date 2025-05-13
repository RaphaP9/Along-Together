using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackSpeedChanged += SpecificPlayerStatsResolver_OnPlayerAttackSpeedChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackSpeedChanged -= SpecificPlayerStatsResolver_OnPlayerAttackSpeedChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleFloat(currentValue, 2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseAttackSpeed;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.AttackSpeed;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerAttackSpeedChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
