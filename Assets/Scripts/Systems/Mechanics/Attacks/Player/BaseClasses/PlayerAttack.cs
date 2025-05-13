using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : EntityAttack
{
    [Header("Player Attack Components")]
    [SerializeField] protected CharacterIdentifier characterIdentifier;
    [SerializeField] protected PlayerAimDirectionerHandler aimDirectionerHandler;

    [Header("Player Attack Settings")]
    [SerializeField] protected AttackTriggerType attackTriggerType;
    [SerializeField] protected LayerMask attackLayermask;

    public AttackTriggerType AttackTriggerType_ => attackTriggerType;
    public enum AttackTriggerType {Automatic, SemiAutomatic}

    #region Events
    public event EventHandler<OnPlayerAttackEventArgs> OnPlayerAttack;
    public static event EventHandler<OnPlayerAttackEventArgs> OnAnyPlayerAttack;
    #endregion

    #region EventArgs Classes
    public class OnPlayerAttackEventArgs : OnEntityAttackEventArgs
    {
        public PlayerAttack playerAttack;
    }
    #endregion

    protected virtual void OnEnable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated += AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated += AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated += AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected virtual void OnDisable()
    {
        AttackDamageStatResolver.OnAttackDamageResolverUpdated -= AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated -= AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated -= AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;
    }

    protected override void HandleAttack()
    {
        if (!GetAttackInput()) return;
        if (!CanAttack()) return;

        Attack();
        MaxTimer();
    }


    protected override void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        base.OnEntityAttackMethod(isCrit, attackDamage);

        OnPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
        OnAnyPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
    }

    #region Stat Calculations
    protected override int CalculateAttackDamage() => AttackDamageStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseAttackDamage);
    protected override float CalculateAttackSpeed() => AttackSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackSpeed);
    protected override float CalculateAttackCritChance() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritChance);
    protected override float CalculateAttackCritDamageMultiplier() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);

    protected override void RecalculateAttackDamage()
    {
        attackDamage = CalculateAttackDamage();
    }

    protected override void RecalculateAttackSpeed()
    {
        attackSpeed = CalculateAttackSpeed();
    }

    protected override void RecalculateAttackCritChance()
    {
        attackCritChance = CalculateAttackCritChance();
    }

    protected override void RecalculateAttackCritDamageMultiplier()
    {
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
    }
    #endregion

    #region AttackTriggerType-Input Assignation
    protected bool GetSemiAutomaticInputAttack() => AttackInput.Instance.GetAttackDown();
    protected bool GetAutomaticInputAttack() => AttackInput.Instance.GetAttackHold();

    protected bool GetAttackInput()
    {
        switch (attackTriggerType)
        {
            case AttackTriggerType.SemiAutomatic:
            default:
                return GetSemiAutomaticInputAttack();
            case AttackTriggerType.Automatic:
                return GetAutomaticInputAttack();
        }
    }
    #endregion

    #region Subscriptions
    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackDamage();
    }
    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackSpeed();
    }
    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritChance();
    }
    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritDamageMultiplier();
    }
    #endregion
}
