using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificEnemyStatResolver : SpecificEntityStatsResolver
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    #region Events
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyStatsUpdated;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyMaxHealthChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyMaxShieldChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyArmorChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyDodgeChanceChanged;
    #endregion

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }

    #region StatCalculation
    protected override int CalculateMaxHealth() => enemyIdentifier.EnemySO.baseHealth;
    protected override int CalculateMaxShield() => enemyIdentifier.EnemySO.baseShield;
    protected override int CalculateArmor() => enemyIdentifier.EnemySO.baseArmor;
    protected override float CalculateDodgeChance() => enemyIdentifier.EnemySO.baseDodgeChance;
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnEnemyStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnEnemyStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnEnemyMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnEnemyMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnEnemyArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnEnemyDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyEnemyDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }
    #endregion
}
