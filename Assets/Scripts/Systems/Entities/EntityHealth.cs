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

    public void HealCompletely(IHealSource healSource)
    {
        
    }

    public void RestoreShieldCompletely(IShieldSource shieldSource)
    {

    }

    public void InstaKill(IDamageSource damageSource)
    {

    }

    public void Heal(int healAmount, IHealSource healSource)
    {
        throw new NotImplementedException();
    }

    public void RestoreShield(int shieldAmount, IShieldSource healSource)
    {
        throw new NotImplementedException();
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

    protected virtual void OnDeathMethod()
    {

    }
    #endregion
}
