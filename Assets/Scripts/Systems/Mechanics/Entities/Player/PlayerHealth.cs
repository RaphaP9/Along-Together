using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    [Header("PlayerHealth Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;

    public static event EventHandler<OnEntityStatsEventArgs> OnPlayerStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnThisPlayerStatsInitialized;

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

    protected override int CalculateCurrentHealth()
    {
        return MaxHealthStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.healthPoints); //Load Value from Static RuntimeData
    }

    protected override int CalculateCurrentShield()
    {
        return MaxShieldStatResolver.Instance.ResolveStatInt(characterIdentifier.CharacterSO.shieldPoints); //Load Value from Static RuntimeData
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

        OnThisPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnPlayerStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
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

        OnThisPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnThisPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnThisPlayerHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnPlayerEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnThisPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnThisPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
        OnPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}