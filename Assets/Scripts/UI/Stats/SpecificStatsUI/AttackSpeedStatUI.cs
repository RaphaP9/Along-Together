using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedStatUI : PlayerNumericStatUI<PlayerAttackSpeedStatResolver>
{
    protected override void SubscribeToEvents()
    {
        resolver.OnEntityStatInitialized += Resolver_OnEntityStatInitialized;
        resolver.OnEntityStatUpdated += Resolver_OnEntityStatUpdated;
    }
    protected override void UnSubscribeToEvents()
    {
        if (resolver == null) return;

        resolver.OnEntityStatInitialized += Resolver_OnEntityStatInitialized;
        resolver.OnEntityStatUpdated -= Resolver_OnEntityStatUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleFloat(currentValue, 2);
    protected override float GetBaseValue() => resolver.BaseValue;
    protected override float GetCurrentValue() => resolver.Value;


    #region Subscriptions
    private void Resolver_OnEntityStatInitialized(object sender, EntityFloatStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void Resolver_OnEntityStatUpdated(object sender, EntityFloatStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}