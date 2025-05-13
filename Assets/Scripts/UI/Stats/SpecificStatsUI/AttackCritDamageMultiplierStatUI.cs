using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackCritDamageMultiplierChanged += SpecificPlayerStatsResolver_OnPlayerAttackCritDamageMultiplierChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackCritDamageMultiplierChanged -= SpecificPlayerStatsResolver_OnPlayerAttackCritDamageMultiplierChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToPercentage(currentValue, 2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.AttackCritDamageMultiplier;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerAttackCritDamageMultiplierChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
