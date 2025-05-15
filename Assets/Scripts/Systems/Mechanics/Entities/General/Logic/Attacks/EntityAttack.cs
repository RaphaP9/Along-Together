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

    protected abstract void Attack();
    private void GetAttackInterruptionInterfaces() => attackInterruptions = GeneralUtilities.TryGetGenericsFromComponents<IAttackInterruption>(attackInterruptionComponents);
    protected bool HasValidAttackSpeed() => entityAttackSpeedStatResolver.Value > 0f;

    protected virtual bool CanAttack()
    {
        if (!entityHealth.IsAlive()) return false;
        if (!HasValidAttackSpeed()) return false;

        foreach (IAttackInterruption attackInterruptionAbility in attackInterruptions)
        {
            if (attackInterruptionAbility.IsInterruptingAttack()) return false;
        }

        return true;
    }

    #region Virtual Event Methods
    protected virtual void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        OnEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
        OnAnyEntityAttack?.Invoke(this, new OnEntityAttackEventArgs {isCrit = isCrit, attackDamage = attackDamage, attackSpeed = entityAttackSpeedStatResolver.Value, attackCritChance = entityAttackCritChanceStatResolver.Value, attackCritDamageMultiplier = entityAttackCritDamageMultiplierStatResolver.Value });
    }
    #endregion
}
