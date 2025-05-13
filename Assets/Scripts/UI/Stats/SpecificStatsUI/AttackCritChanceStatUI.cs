using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatUI : PlayerNumericStatUI
{
    protected override void SubscribeToEvents()
    {
        specificPlayerStatsResolver.OnPlayerStatsInitialized += SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackCritChanceChanged += SpecificPlayerStatsResolver_OnPlayerAttackCritChanceChanged;
    }

    protected override void UnSubscribeToEvents()
    {
        if (specificPlayerStatsResolver == null) return;

        specificPlayerStatsResolver.OnPlayerStatsInitialized -= SpecificPlayerStatsResolver_OnPlayerStatsInitialized;
        specificPlayerStatsResolver.OnPlayerAttackCritChanceChanged -= SpecificPlayerStatsResolver_OnPlayerAttackCritChanceChanged;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToPercentage(currentValue, 2);
    protected override float GetBaseValue() => characterIdentifier.CharacterSO.baseAttackCritChance;
    protected override float GetCurrentValue() => specificPlayerStatsResolver.AttackCritChance;


    #region Subscriptions
    private void SpecificPlayerStatsResolver_OnPlayerAttackCritChanceChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void SpecificPlayerStatsResolver_OnPlayerStatsInitialized(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
