using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecificEntityStatsResolver : MonoBehaviour
{
    [Header("Runtime Filled")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxShield;
    [SerializeField] protected int armor;
    [SerializeField] protected float dodgeChance;
    [Space]
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCritChance;
    [SerializeField] protected float attackCritDamageMultiplier;
    [Space]
    [SerializeField] protected float movementSpeed;
    [Space]
    [SerializeField] protected float lifesteal;

    #region Properties
    public int MaxHealth => maxHealth;
    public int MaxShield => maxShield;
    public int Armor => armor;
    public float DodgeChance => dodgeChance;

    public int AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float AttackCritChance => attackCritChance;
    public float AttackCritDamageMultiplier => attackCritDamageMultiplier;

    public float MovementSpeed => movementSpeed;

    public float Lifesteal => lifesteal;
    #endregion

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsUpdated;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMaxHealthChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMaxShieldChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityArmorChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityDodgeChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityAttackDamageChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityAttackDamageChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityAttackSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityAttackSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityAttackCritChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityAttackCritChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityAttackCritDamageMultiplierChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMovementSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityLifestealChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityLifestealChanged;
    #endregion

    #region EventArgs Classes
    public class OnEntityStatsEventArgs : EventArgs
    {
        public int maxHealth;
        public int maxShield;
        public int armor;
        public float dodgeChance;

        public int attackDamage;
        public float attackSpeed;
        public float attackCritChance;
        public float attackCritDamageMultiplier;

        public float movementSpeed;

        public float lifesteal;
    }
    #endregion

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        maxHealth = CalculateMaxHealth();
        maxShield = CalculateMaxShield();
        armor = CalculateArmor();
        dodgeChance = CalculateDodgeChance();

        attackDamage = CalculateAttackDamage();
        attackSpeed = CalculateAttackSpeed();
        attackCritChance = CalculateAttackCritChance();
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();

        movementSpeed = CalculateMovementSpeed();
        
        lifesteal = CalculateLifesteal();

        OnEntityStatsInitializedMethod();
    }

    protected OnEntityStatsEventArgs GenerateCurrentEntityStatsEventArgs()
    {
        return new OnEntityStatsEventArgs
        {
            maxHealth = maxHealth,
            maxShield = maxShield,
            armor = armor,
            dodgeChance = dodgeChance,
            attackDamage = attackDamage,
            attackSpeed = attackSpeed,
            attackCritChance = attackCritChance,
            attackCritDamageMultiplier = attackCritDamageMultiplier,
            movementSpeed = movementSpeed,
            lifesteal = lifesteal
        };
    }

    #region Stat Calculations
    protected abstract int CalculateMaxHealth();
    protected abstract int CalculateMaxShield();
    protected abstract int CalculateArmor();
    protected abstract float CalculateDodgeChance();

    protected abstract int CalculateAttackDamage();
    protected abstract float CalculateAttackSpeed();
    protected abstract float CalculateAttackCritChance();
    protected abstract float CalculateAttackCritDamageMultiplier();

    protected abstract float CalculateMovementSpeed();

    protected abstract float CalculateLifesteal();
    #endregion

    #region Stat Recalculation
    protected virtual void RecalculateMaxHealth()
    {
        maxHealth = CalculateMaxHealth();
        OnEntityMaxHealthChangedMethod();
    }

    protected virtual void RecalculateMaxShield()
    {
        maxShield = CalculateMaxShield();
        OnEntityMaxShieldChangedMethod();
    }

    protected virtual void RecalculateArmor()
    {
        armor = CalculateArmor();
        OnEntityArmorChangedMethod();
    }

    protected virtual void RecalculateDodgeChance()
    {
        dodgeChance = CalculateDodgeChance();
        OnEntityDodgeChanceChangedMethod();
    }

    protected virtual void RecalculateAttackDamage()
    {
        attackDamage = CalculateAttackDamage();
        OnEntityAttackDamageChangedMethod();
    }

    protected virtual void RecalculateAttackSpeed()
    {
        attackSpeed = CalculateAttackSpeed();
        OnEntityAttackSpeedChangedMethod();
    }

    protected virtual void RecalculateAttackCritChance()
    {
        attackCritChance = CalculateAttackCritChance();
        OnEntityAttackCritChanceChangedMethod();
    }

    protected virtual void RecalculateAttackCritDamageMultiplier()
    {
        attackCritDamageMultiplier = CalculateAttackCritDamageMultiplier();
        OnEntityAttackCritDamageMultiplierChangedMethod();
    }

    protected virtual void RecalculateMovementSpeed()
    {
        movementSpeed = CalculateMovementSpeed();
        OnEntityMovementSpeedChangedMethod();
    }

    protected virtual void RecalculateLifesteal()
    {
        lifesteal = CalculateLifesteal();
        OnEntityLifestealChangedMethod();
    }
    #endregion

    #region Virtual Events Methods
    protected virtual void OnEntityStatsInitializedMethod()
    {
        OnEntityStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityStatsUpdatedMethod()
    {
        OnEntityStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityMaxHealthChangedMethod()
    {
        OnEntityMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityMaxShieldChangedMethod()
    {
        OnEntityMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityArmorChangedMethod()
    {
        OnEntityArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityDodgeChanceChangedMethod()
    {
        OnEntityDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityAttackDamageChangedMethod()
    {
        OnEntityAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityAttackSpeedChangedMethod()
    {
        OnEntityAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityAttackCritChanceChangedMethod()
    {
        OnEntityAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        OnEntityAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityMovementSpeedChangedMethod()
    {
        OnEntityMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected virtual void OnEntityLifestealChangedMethod()
    {
        OnEntityLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEntityLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }
    #endregion
}
