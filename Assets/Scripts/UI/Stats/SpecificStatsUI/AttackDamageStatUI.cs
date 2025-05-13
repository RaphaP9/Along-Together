using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackDamageChanged += SpecificPlayerStatsResolver_OnPlayerAttackDamageChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackDamageChanged -= SpecificPlayerStatsResolver_OnPlayerAttackDamageChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseAttackDamage;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.AttackDamage;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerAttackDamageChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}