using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCritChanceStatUI : NumericStatUI
{
    private void OnEnable()
    {
        AttackCritChanceStatResolver.OnAttackCritChanceResolverInitialized += AttackCritChanceStatResolver_OnAttackCritChanceResolverInitialized;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated += AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
    }

    private void OnDisable()
    {
        AttackCritChanceStatResolver.OnAttackCritChanceResolverInitialized -= AttackCritChanceStatResolver_OnAttackCritChanceResolverInitialized;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated -= AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
    }

    protected override string ProcessCurrentValue(float currentValue) => MechanicsUtilities.ProcessCurrentValueToPercentage(currentValue, 2);
    protected override float GetBaseValue() => PlayerCharacterManager.Instance.CharacterSO.baseAttackCritChance;
    protected override float GetCurrentValue() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(PlayerCharacterManager.Instance.CharacterSO.baseAttackCritChance);


    #region Subscriptions
    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverInitialized(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }

    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        UpdateUIByNewValue(GetCurrentValue(), GetBaseValue());
    }
    #endregion
}
