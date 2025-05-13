using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyPlayerInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnPlayerInitialized;

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

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyPlayerCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnPlayerCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyPlayerCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnPlayerCurrentShieldClamped;
    #endregion

    public void SetCurrentHealth(int setterHealth) => currentHealth = setterHealth;
    public void SetCurrentShield(int setterShield) => currentShield = setterShield;

    #region Virtual Event Methods

    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnPlayerInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyPlayerInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
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
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyPlayerShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnPlayerHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, healSource = healSource, healReceiver = this});
        OnAnyPlayerEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyPlayerShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyPlayerEntityDeath?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnPlayerCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth });
        OnAnyPlayerCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnPlayerCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
        OnAnyPlayerCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
    }
    #endregion
}