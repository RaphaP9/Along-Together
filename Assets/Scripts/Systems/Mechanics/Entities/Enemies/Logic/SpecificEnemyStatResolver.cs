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

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyAttackDamageChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyAttackDamageChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyAttackSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyAttackSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyAttackCritChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyAttackCritChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyAttackCritDamageMultiplierChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyMovementSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyLifestealChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyLifestealChanged;
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

    protected override int CalculateAttackDamage() => enemyIdentifier.EnemySO.baseAttackDamage;
    protected override float CalculateAttackSpeed() => enemyIdentifier.EnemySO.baseAttackSpeed;
    protected override float CalculateAttackCritChance() => enemyIdentifier.EnemySO.baseAttackCritChance;
    protected override float CalculateAttackCritDamageMultiplier() => enemyIdentifier.EnemySO.baseAttackCritDamageMultiplier;

    protected override float CalculateMovementSpeed() => enemyIdentifier.EnemySO.baseMovementSpeed;
    protected override float CalculateLifesteal() => enemyIdentifier.EnemySO.baseLifesteal;
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnEnemyStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnEnemyStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnEnemyMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnEnemyMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnEnemyArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnEnemyDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackDamageChangedMethod()
    {
        base.OnEntityAttackDamageChangedMethod();

        OnEnemyAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackSpeedChangedMethod()
    {
        base.OnEntityAttackSpeedChangedMethod();

        OnEnemyAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritChanceChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnEnemyAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnEnemyAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnEnemyMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityLifestealChangedMethod()
    {
        base.OnEntityLifestealChangedMethod();

        OnEnemyLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyEnemyLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }
    #endregion
}
