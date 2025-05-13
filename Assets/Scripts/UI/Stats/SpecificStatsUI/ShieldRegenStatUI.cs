using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRegenStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerShieldRegenChanged += SpecificPlayerStatsResolver_OnPlayerShieldRegenChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerShieldRegenChanged -= SpecificPlayerStatsResolver_OnPlayerShieldRegenChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseShieldRegen;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.ShieldRegen;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerShieldRegenChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}