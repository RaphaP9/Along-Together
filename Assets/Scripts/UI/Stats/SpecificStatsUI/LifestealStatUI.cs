using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifestealStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerLifestealChanged += SpecificPlayerStatsResolver_OnPlayerLifestealChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerLifestealChanged -= SpecificPlayerStatsResolver_OnPlayerLifestealChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToPercentage(currentValue, 2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseLifesteal;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.Lifesteal;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerLifestealChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}