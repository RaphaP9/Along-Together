using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificPlayerStatsResolver : SpecificEntityStatsResolver
{
    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;

    #region Events
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsUpdated;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxHealthChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxShieldChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerArmorChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerDodgeChanceChanged;
    #endregion

    private void OnEnable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated += MaxHealthStatResolver_OnMaxHealthResolverUpdated;
        MaxShieldStatResolver.OnMaxShieldResolverUpdated += MaxShieldStatResolver_OnMaxShieldResolverUpdated;
        ArmorStatResolver.OnArmorResolverUpdated += ArmorStatResolver_OnArmorResolverUpdated;
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated += DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;
    }

    private void OnDisable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated -= MaxHealthStatResolver_OnMaxHealthResolverUpdated;
        MaxShieldStatResolver.OnMaxShieldResolverUpdated -= MaxShieldStatResolver_OnMaxShieldResolverUpdated;
        ArmorStatResolver.OnArmorResolverUpdated -= ArmorStatResolver_OnArmorResolverUpdated;
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated -= DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;
    }

    protected override int CalculateMaxHealth() => MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseHealth);
    protected override int CalculateMaxShield() => MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseShield);
    protected override int CalculateArmor() => ArmorStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseArmor);
    protected override float CalculateDodgeChance() => DodgeChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseDodgeChance);

    #region Subscriptions
    private void MaxHealthStatResolver_OnMaxHealthResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateMaxHealth();
    }
    private void MaxShieldStatResolver_OnMaxShieldResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateMaxShield();
    }
    private void ArmorStatResolver_OnArmorResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateArmor();
    }

    private void DodgeChanceStatResolver_OnDodgeChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateDodgeChance();
    }
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance});
        OnAnyPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
        OnAnyPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, maxShield = maxShield, armor = armor, dodgeChance = dodgeChance });
    }
    #endregion
}
