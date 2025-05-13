using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAttack : MonoBehaviour
{
    [Header("Entity Attack Components")]
    [SerializeField] protected SpecificEntityStatsResolver specificEntityStatsResolver;
    [Space]
    [SerializeField] protected List<Transform> attackInterruptionAbilitiesTransforms;

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

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetAttackTimer() => attackTimer = 0f;
    protected void MaxTimer() => attackTimer = 1f / specificEntityStatsResolver.AttackSpeed;

    #region Virtual Event Methods
    protected virtual void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        OnEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = specificEntityStatsResolver.AttackSpeed, attackCritChance = specificEntityStatsResolver.AttackCritChance, attackCritDamageMultiplier = specificEntityStatsResolver.AttackCritDamageMultiplier });
        OnAnyEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = specificEntityStatsResolver.AttackSpeed, attackCritChance = specificEntityStatsResolver.AttackCritChance, attackCritDamageMultiplier = specificEntityStatsResolver.AttackCritDamageMultiplier });
    }
    #endregion

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
