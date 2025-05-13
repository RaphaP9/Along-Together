using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability, IActiveAbility
{
    [Header("Active Ability Components")]
    [SerializeField] protected SpecificPlayerStatsResolver specificPlayerStatsResolver;
    [SerializeField] protected AbilityCooldownHandler abilityCooldownHandler;

    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;
    private ActiveAbilitySO ActiveAbilitySO => abilitySO as ActiveAbilitySO;
    public float ProcessedAbilityCooldown => specificPlayerStatsResolver.GetAbilityCooldown(ActiveAbilitySO.baseCooldown);

    #region InterfaceMethods
    public float CalculateAbilityCooldown() => specificPlayerStatsResolver.GetAbilityCooldown(ActiveAbilitySO.baseCooldown);
    public bool AbilityCastInput() => abilitySlotHandler.GetAssociatedDownInput();
    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }
    #endregion

    #region Abstract Methods
    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        abilityCooldownHandler.SetCooldownTimer(ProcessedAbilityCooldown); 
    }

    protected override void OnAbilityCastDeniedMethod()
    {
        base.OnAbilityCastDeniedMethod();
    }

    protected override void OnAbilityVariantActivationMethod()
    {
        abilityCooldownHandler.ResetCooldownTimer(); //Reset Cooldown On Activation
    }

    protected override void OnAbilityVariantDeactivationMethod()
    {
        //
    }
    #endregion
}
