using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenStatUI : PlayerNumericStatUI<PlayerHealthRegenStatResolver>
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

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => resolver.BaseValue;
    protected override float GetCurrentValue() => resolver.Value;


    #region Subscriptions
    private void Resolver_OnEntityStatInitialized(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void Resolver_OnEntityStatUpdated(object sender, EntityIntStatResolver.OnStatEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}