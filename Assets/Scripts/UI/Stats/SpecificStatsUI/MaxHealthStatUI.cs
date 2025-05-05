using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthStatUI : NumericStatUI
{
    private void OnEnable()
    {
        MaxHealthStatResolver.OnMaxHealtResolverInitialized += MaxHealthStatResolver_OnMaxHealtResolverInitialized;
        MaxHealthStatResolver.OnMaxHealthResolverUpdated += MaxHealthStatResolver_OnMaxHealthResolverUpdated;
    }

    private void OnDisable()
    {
        MaxHealthStatResolver.OnMaxHealtResolverInitialized -= MaxHealthStatResolver_OnMaxHealtResolverInitialized;
        MaxHealthStatResolver.OnMaxHealthResolverUpdated -= MaxHealthStatResolver_OnMaxHealthResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleString(currentValue);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseHealth;
    protected override float GetCurrentValue() => MaxHealthStatResolver.Instance.ResolveStatInt(PlayerCharacterManager.Instance.CharacterSO.baseHealth);


    #region Subscriptions
    private void MaxHealthStatResolver_OnMaxHealtResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void MaxHealthStatResolver_OnMaxHealthResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
