using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    [Header("PlayerHealth Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerStatsUpdated;

    public static event EventHandler<OnEntityDodgeEventArgs> OnPlayerDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnThisPlayerEntityDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnPlayerHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnThisPlayerHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnPlayerShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnThisPlayerShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnPlayerEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnThisPlayerHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnPlayerShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnThisPlayerShieldRestored;

    public static event EventHandler OnPlayerEntityDeath;
    public event EventHandler OnThisPlayerEntityDeath;

    //

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerMaxHealthChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerMaxShieldChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerArmorChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerDodgeChanceChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerCurrentHealthClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerCurrentHealthClamped;
    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerCurrentShieldClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerCurrentShieldClamped;
    #endregion

    protected override void CheckStatChanges()
    {
        base.CheckStatChanges();

        SaveStaticData();
    }

    protected override int CalculateStartingCurrentHealth()
    {
        if(RuntimeRunData.CurrentHealth > 0) return RuntimeRunData.CurrentHealth; //If data is O, JSON did not load any health value or health value was default. Should initialize by resolving the MaxHealthStat
        else return MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.healthPoints); 
    }

    protected override int CalculateStartingCurrentShield()
    {
        if (RuntimeRunData.CurrentShield > 0) return RuntimeRunData.CurrentShield;
        else return MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.shieldPoints); //Load Value from Static RuntimeData
    }

    protected override int CalculateMaxHealth()
    {
        return MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.healthPoints);
    }

    protected override int CalculateMaxShield()
    {
        return MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.shieldPoints);
    }

    protected override int CalculateArmor()
    {
        return ArmorStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.armorPoints);
    }

    protected override float CalculateDodgeChance()
    {
        return DodgeChanceStatResolver.Instance.ResolveStatFloat(characterIdentifier.CharacterSO.dodgeChance);
    }

    #region Virtual Methods
    
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        SaveStaticData();

        OnThisPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityStatsUpdatedMethod()
    {
        base.OnEntityStatsUpdatedMethod();

        SaveStaticData();

        OnThisPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }


    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnThisPlayerEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnPlayerDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        SaveStaticData(); //Whenever there is a change, save Static Data

        OnThisPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        SaveStaticData();

        OnThisPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        SaveStaticData();

        OnThisPlayerHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnPlayerEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        SaveStaticData();

        OnThisPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnThisPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
        OnPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
    }

     //

    protected override void OnEntityMaxHealthChangedMethod()
    {
        base.OnEntityMaxHealthChangedMethod();

        OnThisPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityMaxShieldChangedMethod()
    {
        base.OnEntityMaxShieldChangedMethod();

        OnThisPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityArmorChangedMethod()
    {
        base.OnEntityArmorChangedMethod();

        OnThisPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityDodgeChanceChangedMethod()
    {
        base.OnEntityDodgeChanceChangedMethod();

        OnThisPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnThisPlayerCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnThisPlayerCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }
    #endregion

    protected void SaveStaticData()
    {
        RuntimeRunData.CurrentHealth = currentHealth; 
        RuntimeRunData.CurrentShield = currentShield;
    }
}