using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : Ability, IActiveAbility
{
    [Header("Active Ability Components")]
    [SerializeField] protected AbilityCooldownHandler abilityCooldownHandler;

    [Header("Ability Runtime Filled")]
    [SerializeField] protected float abilityCooldownTime;

    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;
    private ActiveAbilitySO ActiveAbilitySO => abilitySO as ActiveAbilitySO;


    protected override void OnEnable()
    {
        base.OnEnable();
        CooldownStatResolver.OnCooldownResolverUpdated += CooldownStatResolver_OnCooldownResolverUpdated;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CooldownStatResolver.OnCooldownResolverUpdated -= CooldownStatResolver_OnCooldownResolverUpdated;
    }

    protected virtual void Start()
    {
        InitializeActiveAbility();
    }

    private void InitializeActiveAbility()
    {
        abilityCooldownTime = CalculateAbilityCooldown();
    }

    private void RecalculateAbilityCooldownTime()
    {
        abilityCooldownTime = CalculateAbilityCooldown();
    }

    #region InterfaceMethods
    public float CalculateAbilityCooldown() => CooldownStatResolver.Instance.ResolveStatFloat(ActiveAbilitySO.baseCooldown);
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
        abilityCooldownHandler.SetCooldownTimer(abilityCooldownTime); 
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

    #region Subscriptions
    private void CooldownStatResolver_OnCooldownResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAbilityCooldownTime();
    }
    #endregion
}
