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
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnEnemyStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }
    #endregion
}
