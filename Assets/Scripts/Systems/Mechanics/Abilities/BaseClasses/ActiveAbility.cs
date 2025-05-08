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

    protected override void Start()
    {
        base.Start();
        InitializeaActiveAbility();
    }

    private void InitializeaActiveAbility()
    {
        abilityCooldownTime = CalculateAbilityCooldown();
    }

    private void RecalculateAbilityCooldownTime()
    {
        abilityCooldownTime = CalculateAbilityCooldown();
    }

    public float CalculateAbilityCooldown() => CooldownStatResolver.Instance.ResolveStatFloat(ActiveAbilitySO.baseCooldown);
    public bool AbilityCastInput() => GetAssociatedDownInput();
    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();
        abilityCooldownHandler.SetCooldownTimer(abilityCooldownTime); 
    }

    #region Subscriptions
    private void CooldownStatResolver_OnCooldownResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAbilityCooldownTime();
    }
    #endregion
}
