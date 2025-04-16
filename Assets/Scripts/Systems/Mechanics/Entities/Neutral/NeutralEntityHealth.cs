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
    protected override int CalculateStartingCurrentHealth()
    {
        return neutralEntityIdentifier.NeutralEntitySO.healthPoints;
    }

    protected override int CalculateStartingCurrentShield()
    {
        return neutralEntityIdentifier.NeutralEntitySO.shieldPoints;
    }

    protected override int CalculateMaxHealth()
    {
        return neutralEntityIdentifier.NeutralEntitySO.healthPoints;
    }

    protected override int CalculateMaxShield()
    {
        return neutralEntityIdentifier.NeutralEntitySO.shieldPoints;
    }

    protected override int CalculateArmor()
    {
        return neutralEntityIdentifier.NeutralEntitySO.armorPoints;
    }

    protected override float CalculateDodgeChance()
    {
        return neutralEntityIdentifier.NeutralEntitySO.dodgeChance;
    }

    #region Virtual Methods
    
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        OnNeutralEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyNeutralEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnAnyNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
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

        OnNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }
    #endregion
}