using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedStatUI : NumericStatUI
{
    private void OnEnable()
    {
        AttackSpeedStatResolver.OnAttackSpeedResolverInitialized += AttackSpeedStatResolver_OnAttackSpeedResolverInitialized;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated += AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
    }

    private void OnDisable()
    {
        AttackSpeedStatResolver.OnAttackSpeedResolverInitialized -= AttackSpeedStatResolver_OnAttackSpeedResolverInitialized;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated -= AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleFloat(currentValue,2);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseAttackSpeed;
    protected override float GetCurrentValue() => AttackSpeedStatResolver.Instance.ResolveStatFloat(PlayerCharacterManager.Instance.CharacterSO.baseAttackSpeed);


    #region Subscriptions
    private void AttackSpeedStatResolver_OnAttackSpeedResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
