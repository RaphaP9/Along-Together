using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    [Header("PlayerHealth Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsUpdated;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyPlayerDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnPlayerEntityDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyPlayerHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnPlayerHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyPlayerShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnPlayerShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyPlayerEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnPlayerHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyPlayerShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnPlayerShieldRestored;

    public static event EventHandler OnAnyPlayerEntityDeath;
    public event EventHandler OnPlayerEntityDeath;

    //

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxHealthChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxShieldChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerArmorChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerDodgeChanceChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerCurrentHealthClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerCurrentHealthClamped;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyPlayerCurrentShieldClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnPlayerCurrentShieldClamped;
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

    public void SetCurrentHealth(int setterHealth) => currentHealth = setterHealth;
    public void SetCurrentShield(int setterShield) => currentShield = setterShield;

    #region Virtual Event Methods
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }


    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnPlayerEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyPlayerDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = maxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = maxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = maxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = maxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnPlayerHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxShield, healSource = healSource, healReceiver = this});
        OnAnyPlayerEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxShield, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
    }

     //

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnPlayerCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnPlayerCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyPlayerCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
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
    #endregion
}