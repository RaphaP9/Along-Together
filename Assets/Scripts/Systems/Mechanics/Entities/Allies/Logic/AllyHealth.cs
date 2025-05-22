using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyHealth : EntityHealth
{
    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyAllyInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnAllyInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyAllyDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnAllyDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyAllyHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAllyHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyAllyShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAllyShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyAllyHeal;
    public event EventHandler<OnEntityHealEventArgs> OnAllyHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyAllyShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnAllyShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyAllyDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnAllyDeath;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyAllyCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAllyCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyAllyCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAllyCurrentShieldClamped;

    #endregion

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnAllyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyAllyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnAllyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyAllyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnAllyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs
        {
            damageTakenByHealth = damageTakenByHealth,
            previousHealth = previousHealth,
            newHealth = currentHealth,
            maxHealth = entityMaxHealthStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

        OnAnyAllyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs
        {
            damageTakenByHealth = damageTakenByHealth,
            previousHealth = previousHealth,
            newHealth = currentHealth,
            maxHealth = entityMaxHealthStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnAllyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs
        {
            damageTakenByShield = damageTakenByShield,
            previousShield = previousShield,
            newShield = currentShield,
            maxShield = entityMaxShieldStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

        OnAnyAllyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs
        {
            damageTakenByShield = damageTakenByShield,
            previousShield = previousShield,
            newShield = currentShield,
            maxShield = entityMaxShieldStatResolver.Value,
            isCrit = isCrit,
            damageSource = damageSource,
            damageReceiver = this
        });

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnAllyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this });
        OnAnyAllyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this });
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnAllyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyAllyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod(EntitySO entitySO, IDamageSourceSO damageSource)
    {
        base.OnEntityDeathMethod(entitySO, damageSource);

        OnAllyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
        OnAnyAllyDeath?.Invoke(this, new OnEntityDeathEventArgs { entitySO = entitySO as AllySO, damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnAllyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyAllyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnAllyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyAllyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
    }
    #endregion
}
