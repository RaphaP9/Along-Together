using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    [Header("Attack Components")]
    [SerializeField] protected CharacterIdentifier characterIdentifier;
    [SerializeField] protected Transform attackPoint;

    [Header("Attack Settings")]
    [SerializeField] private FireType fireType;

    [Header("Attack Runtime Filled")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackCritChance;
    [SerializeField] private float attackCritDamageMultiplier;

    public FireType FireType_ => fireType;
    public enum FireType {Automatic, SemiAutomatic}

    protected float attackTimer = 0f;

    public class OnPlayerAttackEventArgs : EventArgs
    {
        public PlayerAttack playerAttack;
        public bool isCrit;
    }

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

    protected virtual void Start()
    {
        Initialize();
        ResetAttackTimer();
    }

    protected virtual void Initialize()
    {
        attackDamage = CalculateAttackDamage();
        attackSpeed = CalculateAttackSpeed();
        attackCritChance = CalculateAttackCritChance();
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
    }

    protected virtual void Update()
    {
        HandleAttack();
        HandleAttackCooldown();
    }

    private void HandleAttack()
    {
        if (AttackOnCooldown()) return;
        if (!GetAttackInput()) return;

        Attack();
        ResetTimer();
    }

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    protected abstract void Attack();

    private void ResetAttackTimer() => attackTimer = 0f;
    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetTimer() => attackTimer = 1f / attackSpeed;

    #region Stat Calculations
    private int CalculateAttackDamage() => AttackDamageStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseAttackDamage);
    private float CalculateAttackSpeed() => AttackSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackSpeed);
    private float CalculateAttackCritChance() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritChance);
    private float CalculateAttackCritDamageMultiplier() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);

    #endregion

    #region FireType-Input Assignation
    protected bool GetSemiAutomaticInputAttack() => AttackInput.Instance.GetAttackDown();
    protected bool GetAutomaticInputAttack() => AttackInput.Instance.GetAttackHold();

    protected bool GetAttackInput()
    {
        switch (fireType)
        {
            case FireType.SemiAutomatic:
            default:
                return GetSemiAutomaticInputAttack();
            case FireType.Automatic:
                return GetAutomaticInputAttack();
        }
    }
    #endregion

    #region Subscriptions
    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        CalculateAttackDamage();
    }
    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        CalculateAttackSpeed();
    }
    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        CalculateAttackCritChance();
    }
    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        CalculateAttackCritDamageMultiplier();
    }
    #endregion
}
