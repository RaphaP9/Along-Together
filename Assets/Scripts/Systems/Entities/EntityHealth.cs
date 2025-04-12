using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IHasHealth
{
    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;

    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;

    protected const int INSTA_KILL_DAMAGE = 999;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyEntityDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnEntityDodge;

    public static event EventHandler<OnEntityTakeDamageEventArgs> OnAnyEntityTakeDamage;
    public event EventHandler<OnEntityTakeDamageEventArgs> OnEntityTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEntityHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyEntityShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnEntityShieldRestored;

    public static event EventHandler OnAnyEntityDeath;
    public event EventHandler OnEntityDeath;

    #region EventArgs Classes
    public class OnEntityStatsEventArgs : EventArgs
    {
        public int maxHealth;
        public int maxShield;
        public int armor;
        public float dodgeChance;
    }

    public class OnEntityDodgeEventArgs : EventArgs
    {
        public int damageDodged;
        public bool isCrit;

        public IDamageSource damageSource;
    }

    public class OnEntityTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByHealth;
        public int damageTakenByShield;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public bool isCrit;

        public IDamageSource damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnEntityHealEventArgs : EventArgs
    {
        public int healDone;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public IHealSource healSource;
        public IHasHealth healReceiver;
    }

    public class OnEntityShieldRestoredEventArgs : EventArgs
    {
        public int shieldRestored;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public IShieldSource shieldSource;
        public IHasHealth shieldReceiver;
    }
    #endregion

    protected void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {

    }

    protected abstract int CalculateMaxHealth();
    protected abstract int CalculateMaxShield();
    protected abstract int CalculateArmor();
    protected abstract int CalculateDodgeChance();


    #region Interface Methods

    public abstract bool CanTakeDamage();
    public abstract bool CanHeal();
    public abstract bool CanRestoreShield();
    public void TakeDamage(int damage, bool isCrit, IDamageSource damageSource)
    {
        if(!CanTakeDamage()) return;
        if (!IsAlive()) return;

        bool dodged = MechanicsUtilities.EvaluateDodgeChance(CalculateDodgeChance());

        if (dodged)
        {
            OnEntityDodgeMethod(damage, isCrit, damageSource);
            return;
        }

        int mitigatedDamage = MechanicsUtilities.MitigateDamageByArmor(damage, CalculateArmor());

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield, damageTakenByHealth;

        if (HasShield())
        {
            damageTakenByShield = currentShield < damage ? currentShield : damage; //Shield Absorbs all Damage, Ex: if an entity has 3 Shield and would take 10 damage, it destroys all shield and health does not receive damage at all
            damageTakenByHealth = 0;
        }
        else
        {
            damageTakenByShield = 0;
            damageTakenByHealth = currentHealth < damage ? currentHealth : damage;
        }

        currentShield = currentShield < damageTakenByShield ? 0 : currentShield - damageTakenByShield;
        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentShield - damageTakenByHealth;

        OnEntityTakeDamageMethod(damageTakenByHealth, damageTakenByShield, previousHealth, previousShield, isCrit, damageSource);

        if (!IsAlive()) OnDeathMethod();
    }

    public void InstaKill(IDamageSource damageSource)
    {
        if (!CanTakeDamage()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield = INSTA_KILL_DAMAGE;
        int damageTakenByHealth = INSTA_KILL_DAMAGE;

        currentShield = 0;
        currentHealth = 0;
         
        OnEntityTakeDamageMethod(damageTakenByHealth, damageTakenByShield, previousHealth, previousShield, true, damageSource);

        if (!IsAlive()) OnDeathMethod(); //Will certainly execute
    }

    public void Heal(int healAmount, IHealSource healSource)
    {
        if (!CanHeal()) return;
        if(!IsAlive()) return;

        int previousHealth = currentHealth;

        int effectiveHealAmount = currentHealth + healAmount > CalculateMaxHealth() ? CalculateMaxHealth() - currentHealth : healAmount;
        currentHealth = currentHealth + effectiveHealAmount > CalculateMaxHealth()? CalculateMaxHealth() : currentHealth + effectiveHealAmount;

        OnEntityHealMethod(effectiveHealAmount, previousHealth, healSource);
    }
    public void HealCompletely(IHealSource healSource)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int healAmount = CalculateMaxHealth() - currentHealth;
        currentHealth = CalculateMaxHealth();

        OnEntityHealMethod(healAmount, previousHealth, healSource);
    }

    public void RestoreShield(int shieldAmount, IShieldSource shieldSource)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int effectiveShieldRestored = currentShield + shieldAmount > CalculateMaxShield() ? CalculateMaxShield() - currentShield : shieldAmount;
        currentShield = currentShield + effectiveShieldRestored > CalculateMaxShield() ? CalculateMaxShield() : currentShield + effectiveShieldRestored;

        OnEntityShieldRestoredMethod(effectiveShieldRestored, previousShield, shieldSource);
    }

    public void RestoreShieldCompletely(IShieldSource shieldSource)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int shieldAmount = CalculateMaxShield() - currentShield;
        currentShield = CalculateMaxShield();

        OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);
    }

    public bool IsFullHealth() => currentHealth >= CalculateMaxHealth();
    public bool IsFullShield() => currentShield >= CalculateMaxHealth();
    public bool IsAlive() => currentHealth > 0;
    public bool HasShield() => currentShield > 0;

    #endregion

    #region Abstract Methods
    protected virtual void OnEntityDodgeMethod(int damage, bool isCrit, IDamageSource damageSource)
    {
        OnEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damage, isCrit = isCrit, damageSource = damageSource });
        OnAnyEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damage, isCrit = isCrit, damageSource = damageSource });
    }

    protected virtual void OnEntityTakeDamageMethod(int damageTakenByHealth, int damageTakenByShield, int previousHealth, int previousShield, bool isCrit, IDamageSource damageSource)
    {
        OnEntityTakeDamage?.Invoke(this, new OnEntityTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, damageTakenByShield = damageTakenByShield, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit,
        damageSource = damageSource, damageReceiver = this});

        OnAnyEntityTakeDamage?.Invoke(this, new OnEntityTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, damageTakenByShield = damageTakenByShield, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit,
        damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnEntityHealMethod(int healAmount, int previousHealth, IHealSource healSource)
    {
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected virtual void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSource shieldSource)
    {
        OnEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected virtual void OnDeathMethod()
    {
        OnEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyEntityDeath?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
