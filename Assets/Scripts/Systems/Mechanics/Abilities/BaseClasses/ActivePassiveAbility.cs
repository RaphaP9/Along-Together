using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivePassiveAbility : Ability, IActiveAbility, IPassiveAbility
{
    [Header("Active Ability Components")]
    [SerializeField] protected AbilityCooldownHandler abilityCooldownHandler;

    [Header("Ability Runtime Filled")]
    [SerializeField] protected float abilityCooldownTime;

    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;
    private ActivePassiveAbilitySO ActivePassiveAbilitySO => abilitySO as ActivePassiveAbilitySO;

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

    #region InterfaceMethods
    public float CalculateAbilityCooldown() => CooldownStatResolver.Instance.ResolveStatFloat(ActivePassiveAbilitySO.baseCooldown);
    public bool AbilityCastInput() => GetAssociatedDownInput();
    public override bool CanCastAbility()
    {
        if (!base.CanCastAbility()) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }
    #endregion

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
