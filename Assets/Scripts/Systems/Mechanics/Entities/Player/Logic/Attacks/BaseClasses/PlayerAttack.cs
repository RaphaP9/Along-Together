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

        OnPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyPlayerAttack?.Invoke(this, new OnPlayerAttackEventArgs { playerAttack = this, isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }

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
}
