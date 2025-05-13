using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerMaxHealthChanged += SpecificPlayerStatsResolver_OnPlayerMaxHealthChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerMaxHealthChanged -= SpecificPlayerStatsResolver_OnPlayerMaxHealthChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleFloat(currentValue,2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseHealth;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.MaxHealth;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerMaxHealthChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
