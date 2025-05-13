using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerHealthRegenChanged += SpecificPlayerStatsResolver_OnPlayerHealthRegenChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerHealthRegenChanged -= SpecificPlayerStatsResolver_OnPlayerHealthRegenChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseHealthRegen;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.HealthRegen;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerHealthRegenChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}