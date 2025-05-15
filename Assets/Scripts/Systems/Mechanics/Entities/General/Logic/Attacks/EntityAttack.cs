using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAttack : MonoBehaviour
{
    [Header("Entity Attack Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [Space]
    [SerializeField] protected EntityAttackDamageStatResolver entityAttackDamageStatResolver;
    [SerializeField] protected EntityAttackSpeedStatResolver entityAttackSpeedStatResolver;
    [SerializeField] protected EntityAttackCritChanceStatResolver entityAttackCritChanceStatResolver;
    [SerializeField] protected EntityAttackCritDamageMultiplierStatResolver entityAttackCritDamageMultiplierStatResolver;
    [Space]
    [SerializeField] protected List<Component> attackInterruptionComponents;

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected float attackTimer = 0f;
    private List<IAttackInterruption> attackInterruptions;

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
        GetAttackInterruptionInterfaces();
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
    private void GetAttackInterruptionInterfaces() => attackInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IAttackInterruption>(attackInterruptionComponents);

    private void HandleAttackCooldown()
    {
        if (attackTimer < 0) return;

        attackTimer -= Time.deltaTime;
    }

    private bool AttackOnCooldown() => attackTimer > 0f;
    private void ResetAttackTimer() => attackTimer = 0f;
    private bool HasValidAttackSpeed() => entityAttackSpeedStatResolver.Value > 0f;

    protected void MaxTimer()
    {
        if (!HasValidAttackSpeed()) return;

        attackTimer = 1f / entityAttackSpeedStatResolver.Value;
    } 

    #region Virtual Event Methods
    protected virtual void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        OnEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }
    #endregion

    protected virtual bool CanAttack()
    {
        if (!entityHealth.IsAlive()) return false;
        if (!HasValidAttackSpeed()) return false;
        if (AttackOnCooldown()) return false;

        foreach (IAttackInterruption attackInterruptionAbility in attackInterruptions)
        {
            if (attackInterruptionAbility.IsInterruptingAttack()) return false;
        }

        return true;
    }
}
