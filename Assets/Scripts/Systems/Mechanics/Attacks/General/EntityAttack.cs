using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttack;

public abstract class EntityAttack : MonoBehaviour
{
    [Header("Entity Attack Components")]
    [SerializeField] protected List<Transform> attackInterruptionAbilitiesTransforms;

    [Header("Entity Attack Runtime Filled")]
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCritChance;
    [SerializeField] protected float attackCritDamageMultiplier;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected float attackTimer = 0f;
    private List<IAttackInterruptionAbility> attackInterruptionAbilities;

    #region Events
    public event EventHandler<OnEntityAttackEventArgs> OnEntityAttack;
    public static event EventHandler<OnEntityAttackEventArgs> OnAnyEntityAttack;
    #endregion

    #region EventArgs Classes
    public class OnEntityAttackEventArgs : EventArgs
    {
        public bool isCrit;

        public int attackDamage;
        public float attackSpeed;
        public float attackCritChance;
        public float attackCritDamageMultiplier;
    }
    #endregion



    private void Awake()
    {
        GetAttackInterruptionAbilitiesInterfaces();
    }

    protected virtual void Start()
    {
        Initialize();
        ResetAttackTimer();
    }

    protected virtual void Update()
    {
        HandleAttack();
        HandleAttackCooldown();
    }

    protected abstract void HandleAttack();
    protected abstract void Attack();
    private void GetAttackInterruptionAbilitiesInterfaces() => attackInterruptionAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IAttackInterruptionAbility>(attackInterruptionAbilitiesTransforms);

    protected virtual void Initialize()
    {
        attackDamage = CalculateAttackDamage();
        attackSpeed = CalculateAttackSpeed();
        attackCritChance = CalculateAttackCritChance();
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
    }

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetAttackTimer() => attackTimer = 0f;
    protected void MaxTimer() => attackTimer = 1f / attackSpeed;

    #region Stat Calculations

    protected abstract int CalculateAttackDamage();
    protected abstract float CalculateAttackSpeed();
    protected abstract float CalculateAttackCritChance();
    protected abstract float CalculateAttackCritDamageMultiplier();

    protected abstract void RecalculateAttackDamage();
    protected abstract void RecalculateAttackSpeed();
    protected abstract void RecalculateAttackCritChance();
    protected abstract void RecalculateAttackCritDamageMultiplier();

    #endregion

    protected virtual void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        OnEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
        OnAnyEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = attackSpeed, attackCritChance = attackCritChance, attackCritDamageMultiplier = attackCritDamageMultiplier });
    }

    protected virtual bool CanAttack()
    {
        if (AttackOnCooldown()) return false;

        foreach (IAttackInterruptionAbility attackInterruptionAbility in attackInterruptionAbilities)
        {
            if (attackInterruptionAbility.IsInterruptingAttack()) return false;
        }

        return true;
    }
}
