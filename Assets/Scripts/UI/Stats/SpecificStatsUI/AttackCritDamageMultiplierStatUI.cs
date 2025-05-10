using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritDamageMultiplierStatUI : NumericStatUI
{
    private void OnEnable()
    {
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverInitialized += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverInitialized;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    private void OnDisable()
    {
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverInitialized -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverInitialized;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToPercentage(currentValue, 2);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseAttackCritDamageMultiplier;
    protected override float GetCurrentValue() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(PlayerCharacterManager.Instance.CharacterSO.baseAttackCritDamageMultiplier);


    #region Subscriptions
    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
