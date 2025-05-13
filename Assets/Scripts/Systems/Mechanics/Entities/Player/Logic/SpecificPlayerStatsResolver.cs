using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificPlayerStatsResolver : SpecificEntityStatsResolver
{
    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;

    [Header("Player Runtime Filled")]
    [SerializeField] protected int healthRegen;
    [SerializeField] protected int shieldRegen;
    [SerializeField] protected float cooldownReduction;

    #region Properties
    public int HealthRegen => healthRegen;
    public int ShieldRegen => shieldRegen;
    public float CooldownReduction => cooldownReduction;
    #endregion

    #region Events
    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerStatsInitialized;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerStatsInitialized;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerStatsUpdated;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerStatsUpdated;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerMaxHealthChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerMaxHealthChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerMaxShieldChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerMaxShieldChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerArmorChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerArmorChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerDodgeChanceChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerDodgeChanceChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerAttackDamageChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerAttackDamageChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerAttackSpeedChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerAttackSpeedChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerAttackCritChanceChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerAttackCritChanceChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerAttackCritDamageMultiplierChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerAttackCritDamageMultiplierChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerMovementSpeedChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerMovementSpeedChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerLifestealChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerLifestealChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerHealthRegenChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerHealthRegenChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerShieldRegenChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerShieldRegenChanged;

    public static event EventHandler<OnPlayerStatsEventArgs> OnAnyPlayerCooldownReductionChanged;
    public event EventHandler<OnPlayerStatsEventArgs> OnPlayerCooldownReductionChanged;
    #endregion

    #region EventArgs Classes
    public class OnPlayerStatsEventArgs : OnEntityStatsEventArgs
    {
        public int healthRegen;
        public int shieldRegen;
        public float cooldownReduction;
    }
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

        LifestealStatResolver.OnLifestealResolverUpdated += LifestealStatResolver_OnLifestealResolverUpdated;

        HealthRegenStatResolver.OnHealthRegenResolverUpdated += HealthRegenStatResolver_OnHealthRegenResolverUpdated;
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated += ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
        CooldownReductionStatResolver.OnCooldownResolverUpdated += CooldownReductionStatResolver_OnCooldownResolverUpdated;
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

        LifestealStatResolver.OnLifestealResolverUpdated -= LifestealStatResolver_OnLifestealResolverUpdated;

        HealthRegenStatResolver.OnHealthRegenResolverUpdated -= HealthRegenStatResolver_OnHealthRegenResolverUpdated;
        ShieldRegenStatResolver.OnShieldRegenResolverUpdated -= ShieldRegenStatResolver_OnShieldRegenResolverUpdated;
        CooldownReductionStatResolver.OnCooldownResolverUpdated -= CooldownReductionStatResolver_OnCooldownResolverUpdated;
    }

    protected OnPlayerStatsEventArgs GenerateCurrentPlayerStatsEventArgs()
    {
        return new OnPlayerStatsEventArgs
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
            lifesteal = lifesteal,

            healthRegen = healthRegen,
            shieldRegen = shieldRegen,
            cooldownReduction = cooldownReduction,
        };
    }

    protected override void Initialize()
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

        healthRegen = CalculateHealthRegen();
        shieldRegen = CalculateShieldRegen();
        cooldownReduction = CalculateCooldownReduction();

        OnEntityStatsInitializedMethod();
    }

    #region Stat Calculations
    protected override int CalculateMaxHealth() => MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseHealth);
    protected override int CalculateMaxShield() => MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseShield);
    protected override int CalculateArmor() => ArmorStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseArmor);
    protected override float CalculateDodgeChance() => DodgeChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseDodgeChance);

    protected override int CalculateAttackDamage() => AttackDamageStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseAttackDamage);
    protected override float CalculateAttackSpeed() => AttackSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackSpeed);
    protected override float CalculateAttackCritChance() => AttackCritChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritChance);
    protected override float CalculateAttackCritDamageMultiplier() => AttackCritDamageMultiplierStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseAttackCritDamageMultiplier);

    protected override float CalculateMovementSpeed() => MovementSpeedStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseMovementSpeed);

    protected override float CalculateLifesteal() => LifestealStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseLifesteal);

    protected int CalculateHealthRegen() => HealthRegenStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseHealthRegen);
    protected int CalculateShieldRegen() => ShieldRegenStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.baseShieldRegen);
    protected float CalculateCooldownReduction() => CooldownReductionStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.baseCooldownReduction);

    public float GetAbilityCooldown(float abilityOriginalCooldown)
    {
        float newCooldown = abilityOriginalCooldown * (1 - cooldownReduction);
        newCooldown = newCooldown <= MechanicsUtilities.GetAbilityCooldownMinValue() ? MechanicsUtilities.GetAbilityCooldownMinValue(): newCooldown;

        return newCooldown;
    }
    #endregion

    #region Stat Recalculations
    protected virtual void RecalculateHealthRegen()
    {
        healthRegen = CalculateHealthRegen();
        OnPlayerHealthRegenChangedMethod();
    }

    protected virtual void RecalculateShieldRegen()
    {
        shieldRegen = CalculateShieldRegen();
        OnPlayerShieldRegenChangedMethod();
    }

    protected virtual void RecalculateCooldownReduction()
    {
        cooldownReduction = CalculateCooldownReduction();
        OnPlayerCooldownReductionChangedMethod();
    }
    #endregion

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnPlayerStatsInitialized?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerStatsInitialized?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnPlayerStatsUpdated?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerStatsUpdated?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnPlayerMaxHealthChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerMaxHealthChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnPlayerMaxShieldChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerMaxShieldChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnPlayerArmorChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerArmorChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnPlayerDodgeChanceChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerDodgeChanceChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityAttackDamageChangedMethod()
    {
        base.OnEntityAttackDamageChangedMethod();

        OnPlayerAttackDamageChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerAttackDamageChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityAttackSpeedChangedMethod()
    {
        base.OnEntityAttackSpeedChangedMethod();

        OnPlayerAttackSpeedChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerAttackSpeedChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityAttackCritChanceChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnPlayerAttackCritChanceChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerAttackCritChanceChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityAttackCritDamageMultiplierChangedMethod()
    {
        base.OnEntityAttackCritDamageMultiplierChangedMethod();

        OnPlayerAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerAttackCritDamageMultiplierChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityMovementSpeedChangedMethod()
    {
        base.OnEntityMovementSpeedChangedMethod();

        OnPlayerMovementSpeedChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerMovementSpeedChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected override void OnEntityLifestealChangedMethod()
    {
        base.OnEntityLifestealChangedMethod();

        OnPlayerLifestealChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerLifestealChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected virtual void OnPlayerHealthRegenChangedMethod()
    {
        OnPlayerHealthRegenChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerHealthRegenChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected virtual void OnPlayerShieldRegenChangedMethod()
    {
        OnPlayerShieldRegenChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerShieldRegenChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
    }

    protected virtual void OnPlayerCooldownReductionChangedMethod()
    {
        OnPlayerCooldownReductionChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
        OnAnyPlayerCooldownReductionChanged?.Invoke(this, GenerateCurrentPlayerStatsEventArgs());
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

    private void LifestealStatResolver_OnLifestealResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateLifesteal();
    }

    private void HealthRegenStatResolver_OnHealthRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateHealthRegen();
    }
    private void ShieldRegenStatResolver_OnShieldRegenResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateShieldRegen();
    }

    private void CooldownReductionStatResolver_OnCooldownResolverUpdated(object sender, NumericStatResolver.OnNumericResolverEventArgs e)
    {
        RecalculateCooldownReduction();
    }
    #endregion
}
