using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificNeutralEntityStatResolver : SpecificEntityStatsResolver
{
    [Header("Components")]
    [SerializeField] private NeutralEntityIdentifier neutralEntityIdentifier;

    #region Events
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyStatsUpdated;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityMaxHealthChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityMaxShieldChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityArmorChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityDodgeChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityAttackDamageChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityAttackDamageChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityAttackSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityAttackSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityAttackCritChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityAttackCritChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityAttackCritDamageMultiplierChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityMovementSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityLifestealChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityLifestealChanged;
    #endregion

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    #region StatCalculation
    protected override int CalculateMaxHealth() => neutralEntityIdentifier.NeutralEntitySO.baseHealth;
    protected override int CalculateMaxShield() => neutralEntityIdentifier.NeutralEntitySO.baseShield;
    protected override int CalculateArmor() => neutralEntityIdentifier.NeutralEntitySO.baseArmor;
    protected override float CalculateDodgeChance() => neutralEntityIdentifier.NeutralEntitySO.baseDodgeChance;

    protected override int CalculateAttackDamage() => neutralEntityIdentifier.NeutralEntitySO.baseAttackDamage;
    protected override float CalculateAttackSpeed() => neutralEntityIdentifier.NeutralEntitySO.baseAttackSpeed;
    protected override float CalculateAttackCritChance() => neutralEntityIdentifier.NeutralEntitySO.baseAttackCritChance;
    protected override float CalculateAttackCritDamageMultiplier() => neutralEntityIdentifier.NeutralEntitySO.baseAttackCritDamageMultiplier;

    protected override float CalculateMovementSpeed() => neutralEntityIdentifier.NeutralEntitySO.baseMovementSpeed;

    protected override float CalculateLifesteal() => neutralEntityIdentifier.NeutralEntitySO.baseLifesteal;
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnNeutralEntityStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnEnemyStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnNeutralEntityMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnNeutralEntityMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnNeutralEntityArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnNeutralEntityDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackDamageChangedMethod()
    {
        base.OnEntityAttackDamageChangedMethod();

        OnNeutralEntityAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackSpeedChangedMethod()
    {
        base.OnEntityAttackSpeedChangedMethod();

        OnNeutralEntityAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritChanceChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnNeutralEntityAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnNeutralEntityAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnNeutralEntityMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityLifestealChangedMethod()
    {
        base.OnEntityLifestealChangedMethod();

        OnNeutralEntityLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyNeutralEntityLifestealChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }
    #endregion
}
