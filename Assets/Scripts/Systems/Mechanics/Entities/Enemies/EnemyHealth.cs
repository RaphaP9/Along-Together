using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    [Header("EnemyHealth Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEnemyStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnEnemyStatsInitialized;

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

    public static event EventHandler OnAnyEnemyDeath;
    public event EventHandler OnEnemyDeath;

    protected override int CalculateMaxHealth()
    {
        return enemyIdentifier.EnemySO.healthPoints;
    }

    protected override int CalculateMaxShield()
    {
        return enemyIdentifier.EnemySO.shieldPoints;
    }

    protected override int CalculateArmor()
    {
        return enemyIdentifier.EnemySO.armorPoints;
    }

    protected override float CalculateDodgeChance()
    {
        return enemyIdentifier.EnemySO.dodgeChance;
    }

    #region Virtual Methods
    
    protected override void OnEntityStatsInitializedMethod()
    {
        base.OnEntityStatsInitializedMethod();

        OnEnemyStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEnemyStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }


    protected override void OnEntityDodgeMethod(DamageData damageData)
    {
        base.OnEntityDodgeMethod(damageData);

        OnEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEnemyDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected override void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, isCrit, damageSource);

        OnEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEnemyHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected override void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        base.OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, isCrit, damageSource);

        OnEnemyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEnemyShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected override void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        base.OnEntityHealMethod(healAmount, previousHealth, healSource);

        OnEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyEnemyHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected override void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        base.OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);

        OnEnemyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEnemyShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected override void OnEntityDeathMethod()
    {
        base.OnEntityDeathMethod();

        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        OnAnyEnemyDeath?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
