using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageStatUI : NumericStatUI
{
    private void OnEnable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverInitialized += AttackDamageStatResolver_OnAttackDamageResolverInitialized;
        AttackDamageStatResolver.OnAttackDamageResolverUpdated += AttackDamageStatResolver_OnAttackDamageResolverUpdated;
    }

    private void OnDisable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverInitialized -= AttackDamageStatResolver_OnAttackDamageResolverInitialized;
        AttackDamageStatResolver.OnAttackDamageResolverUpdated -= AttackDamageStatResolver_OnAttackDamageResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToSimpleInt(currentValue);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseAttackDamage;
    protected override float GetCurrentValue() => AttackDamageStatResolver.Instance.ResolveStatInt(PlayerCharacterManager.Instance.CharacterSO.baseAttackDamage);


    #region Subscriptions
    private void AttackDamageStatResolver_OnAttackDamageResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}