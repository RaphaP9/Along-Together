using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificAllyStatsResolver : SpecificEntityStatsResolver
{
    [Header("Components")]
    [SerializeField] private AllyIdentifier allyIdentifier;

    #region Events
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyStatsUpdated;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyMaxHealthChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyMaxShieldChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyArmorChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyDodgeChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyAttackDamageChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyAttackDamageChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyAttackSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyAttackSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyAttackCritChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyAttackCritChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyAttackCritDamageMultiplierChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyMovementSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyAllyLifestealChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnAllyLifestealChanged;
    #endregion

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    #region StatCalculation
    protected override int CalculateMaxHealth() => allyIdentifier.AllySO.baseHealth;
    protected override int CalculateMaxShield() => allyIdentifier.AllySO.baseShield;
    protected override int CalculateArmor() => allyIdentifier.AllySO.baseArmor;
    protected override float CalculateDodgeChance() => allyIdentifier.AllySO.baseDodgeChance;

    protected override int CalculateAttackDamage() => allyIdentifier.AllySO.baseAttackDamage;
    protected override float CalculateAttackSpeed() => allyIdentifier.AllySO.baseAttackSpeed;
    protected override float CalculateAttackCritChance() => allyIdentifier.AllySO.baseAttackCritChance;
    protected override float CalculateAttackCritDamageMultiplier() => allyIdentifier.AllySO.baseAttackCritDamageMultiplier;

    protected override float CalculateMovementSpeed() => allyIdentifier.AllySO.baseMovementSpeed;

    protected override float CalculateLifesteal() => allyIdentifier.AllySO.baseLifesteal;
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnAllyStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnAllyStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnAllyMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnAllyMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnAllyArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnAllyDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackDamageChangedMethod()
    {
        base.OnEntityAttackDamageChangedMethod();

        OnAllyAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackSpeedChangedMethod()
    {
        base.OnEntityAttackSpeedChangedMethod();

        OnAllyAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritChanceChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnAllyAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnAllyAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnAllyMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityLifestealChangedMethod()
    {
        base.OnEntityLifestealChangedMethod();

        OnAllyLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyAllyLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }
    #endregion
}
