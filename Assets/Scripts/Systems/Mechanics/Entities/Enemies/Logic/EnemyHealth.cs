using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [Header("Enemy Health Components")]
    [SerializeField] protected EnemySpawnHandler enemySpawnHandler;

    #region Events
    public static event EventHandler<OnEntityInitializedEventArgs> OnAnyEnemyInitialized;
    public event EventHandler<OnEntityInitializedEventArgs> OnEnemyInitialized;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyEnemyDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnEnemyDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyEnemyHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnEnemyHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyEnemyShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnEnemyShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEnemyHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEnemyHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyEnemyShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnEnemyShieldRestored;

    public static event EventHandler<OnEntityDeathEventArgs> OnAnyEnemyDeath;
    public event EventHandler<OnEntityDeathEventArgs> OnEnemyDeath;

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyEnemyCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnEnemyCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyEnemyCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnEnemyCurrentShieldClamped;
    #endregion

    public override bool AvoidDamagePassThrough()
    {
        if (!IsAlive()) return true;
        if (enemySpawnHandler.IsSpawning) return true;

        return false;
    }

    #region Virtual Event Methods
    protected override void OnEntityInitializedMethod()
    {
        base.OnEntityInitializedMethod();

        OnEnemyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
        OnAnyEnemyInitialized?.Invoke(this, new OnEntityInitializedEventArgs { currentHealth = currentHealth, currentShield = currentShield });
    }

    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnEnemyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEnemyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
        OnAnyEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value, healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnEnemyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEnemyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = entityMaxShieldStatResolver.Value, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod(IDamageSourceSO damageSource)
    {
        base.OnEntityDeathMethod(damageSource);

        OnEnemyDeath?.Invoke(this, new OnEntityDeathEventArgs { damageSource = damageSource });
        OnAnyEnemyDeath?.Invoke(this, new OnEntityDeathEventArgs { damageSource = damageSource });
    }

    protected override void OnEntityCurrentHealthClampedMethod()
    {
        base.OnEntityCurrentHealthClampedMethod();

        OnEnemyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
        OnAnyEnemyCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = entityMaxHealthStatResolver.Value });
    }

    protected override void OnEntityCurrentShieldClampedMethod()
    {
        base.OnEntityCurrentShieldClampedMethod();

        OnEnemyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
        OnAnyEnemyCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = entityMaxShieldStatResolver.Value });
    }
    #endregion
}
