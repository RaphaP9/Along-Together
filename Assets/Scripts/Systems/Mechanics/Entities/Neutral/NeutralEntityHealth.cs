using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEntityHealth : EntityHealth
{
    [Header("NeutralEntityHealth Components")]
    [SerializeField] private NeutralEntityIdentifier neutralEntityIdentifier;

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityStatsUpdated;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyNeutralEntityDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnNeutralEntityDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyNeutralEntityHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnNeutralEntityHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyNeutralEntityShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnNeutralEntityShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyNeutralEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnNeutralEntityHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyNeutralEntityShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnNeutralEntityShieldRestored;

    public static event EventHandler OnAnyNeutralEntityDeath;
    public event EventHandler OnNeutralEntityDeath;

    //

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityMaxHealthChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityMaxShieldChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityArmorChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityDodgeChanceChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityCurrentHealthClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityCurrentHealthClamped;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyNeutralEntityCurrentShieldClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnNeutralEntityCurrentShieldClamped;

    #endregion

    protected override int CalculateMaxHealth() => neutralEntityIdentifier.NeutralEntitySO.baseHealth;
    protected override int CalculateMaxShield() => neutralEntityIdentifier.NeutralEntitySO.baseShield;
    protected override int CalculateArmor() => neutralEntityIdentifier.NeutralEntitySO.baseArmor;
    protected override float CalculateDodgeChance() => neutralEntityIdentifier.NeutralEntitySO.baseDodgeChance;

    #region Virtual Event Methods
    
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnNeutralEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = maxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = maxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = maxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = maxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxHealth, healSource = healSource, healReceiver = this});
        OnAnyNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxHealth, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnNeutralEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyNeutralEntityDeath?.Invoke(this, EventArgs.Empty);
    }

    //

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }
    #endregion
}