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

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerAttackDamageChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerAttackDamageChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerAttackSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerAttackSpeedChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerAttackCritChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerAttackCritChanceChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerAttackCritDamageMultiplierChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerMovementSpeedChanged;
    #endregion

    private void OnEnable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated += MaxHealthStatResolver_OnMaxHealthResolverUpdated;
        MaxShieldStatResolver.OnMaxShieldResolverUpdated += MaxShieldStatResolver_OnMaxShieldResolverUpdated;
        ArmorStatResolver.OnArmorResolverUpdated += ArmorStatResolver_OnArmorResolverUpdated;
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated += DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;

        AttackDamageStatResolver.OnAttackDamageResolverUpdated += AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated += AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated += AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated += AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;

        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated += MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    private void OnDisable()
    {
        MaxHealthStatResolver.OnMaxHealthResolverUpdated -= MaxHealthStatResolver_OnMaxHealthResolverUpdated;
        MaxShieldStatResolver.OnMaxShieldResolverUpdated -= MaxShieldStatResolver_OnMaxShieldResolverUpdated;
        ArmorStatResolver.OnArmorResolverUpdated -= ArmorStatResolver_OnArmorResolverUpdated;
        DodgeChanceStatResolver.OnDodgeChanceResolverUpdated -= DodgeChanceStatResolver_OnDodgeChanceResolverUpdated;

        AttackDamageStatResolver.OnAttackDamageResolverUpdated -= AttackDamageStatResolver_OnAttackDamageResolverUpdated;
        AttackSpeedStatResolver.OnAttackSpeedResolverUpdated -= AttackSpeedStatResolver_OnAttackSpeedResolverUpdated;
        AttackCritChanceStatResolver.OnAttackCritChanceResolverUpdated -= AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated;
        AttackCritDamageMultiplierStatResolver.OnAttackCritDamageMultiplierResolverUpdated -= AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated;

        MovementSpeedStatResolver.OnMovementSpeedResolverUpdated -= MovementSpeedStatResolver_OnMovementSpeedResolverUpdated;
    }

    #region StatCalculation
    protected override int CalculateMaxHealth() => MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseHealth);
    protected override int CalculateMaxShield() => MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseShield);
    protected override int CalculateArmor() => ArmorStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseArmor);
    protected override float CalculateDodgeChance() => DodgeChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseDodgeChance);

    protected override int CalculateAttackDamage() => AttackDamageStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseAttackDamage);
    protected override float CalculateAttackSpeed() => AttackSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackSpeed);
    protected override float CalculateAttackCritChance() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritChance);
    protected override float CalculateAttackCritDamageMultiplier() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);

    protected override float CalculateMovementSpeed() => MovementSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseMovementSpeed);
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnPlayerStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerStatsInitialized?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnPlayerStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerStatsUpdated?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnPlayerMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerMaxHealthChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnPlayerMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerMaxShieldChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnPlayerArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerArmorChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnPlayerDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerDodgeChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackDamageChangedMethod()
    {
        base.OnEntityAttackDamageChangedMethod();

        OnPlayerAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerAttackDamageChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackSpeedChangedMethod()
    {
        base.OnEntityAttackSpeedChangedMethod();

        OnPlayerAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerAttackSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritChanceChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnPlayerAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerAttackCritChanceChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnPlayerAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnPlayerMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
        OnAnyPlayerMovementSpeedChanged?.Invoke(this, GenerateCurrentEntityStatsEventArgs());
    }
    #endregion

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

    private void AttackDamageStatResolver_OnAttackDamageResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackDamage();
    }

    private void AttackSpeedStatResolver_OnAttackSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackSpeed();
    }

    private void AttackCritChanceStatResolver_OnAttackCritChanceResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritChance();
    }
    private void AttackCritDamageMultiplierStatResolver_OnAttackCritDamageMultiplierResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateAttackCritDamageMultiplier();
    }

    private void MovementSpeedStatResolver_OnMovementSpeedResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateMovementSpeed();
    }
    #endregion
}
