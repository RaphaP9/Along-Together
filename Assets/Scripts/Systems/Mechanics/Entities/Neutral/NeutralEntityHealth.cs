using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEntityHealth : EntityHealth
{
    [Header("NeutralEntityHealth Components")]
    [SerializeField] private NeutralEntityIdentifier neutralEntityIdentifier;

    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyNeutralEntityInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnNeutralEntityInitialized;

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

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyNeutralEntityCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnNeutralEntityCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyNeutralEntityCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnNeutralEntityCurrentShieldClamped;

    #endregion

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnNeutralEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyNeutralEntityInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
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
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyNeutralEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, healSource = healSource, healReceiver = this});
        OnAnyNeutralEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyNeutralEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnNeutralEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyNeutralEntityDeath?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth , maxHealth = specificEntityStatsResolver.MaxHealth });
        OnAnyNeutralEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
        OnAnyNeutralEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
    }
    #endregion
}