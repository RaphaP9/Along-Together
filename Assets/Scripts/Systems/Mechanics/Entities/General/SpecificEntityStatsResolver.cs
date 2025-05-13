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

    #region Properties
    public int MaxHealth => maxHealth;
    public int MaxShield => maxShield;
    public int Armor => armor;
    public float DodgeChance => dodgeChance;
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

    #endregion

    #region EventArgs Classes
    public class OnEntityStatsEventArgs : EventArgs
    {
        public int maxHealth;
        public int maxShield;
        public int armor;
        public float dodgeChance;
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

        OnEntityStatsInitializedMethod();
    }

    protected abstract int CalculateMaxHealth();
    protected abstract int CalculateMaxShield();
    protected abstract int CalculateArmor();
    protected abstract float CalculateDodgeChance();

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

    #region Virtual Events Methods
    protected virtual void OnEntityStatsInitializedMethod()
    {
        OnEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance});
        OnAnyEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityStatsUpdatedMethod()
    {
        OnEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected virtual void OnEntityMaxHealthChangedMethod()
    {
        OnEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected virtual void OnEntityMaxShieldChangedMethod()
    {
        OnEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected virtual void OnEntityArmorChangedMethod()
    {
        OnEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected virtual void OnEntityDodgeChanceChangedMethod()
    {
        OnEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }
    #endregion
}
