using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IHasHealth
{
    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;
    [Space]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxShield;
    [SerializeField] protected int armor;
    [SerializeField] protected float dodgeChance;

    #region Properties
    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;
    public int MaxHealth => maxHealth;
    public int MaxShield => maxShield;
    public int Armor => armor;
    public float DodgeChance => dodgeChance;
    #endregion

    #region Events

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsUpdated;

    public static event EventHandler<OnEntityDodgeEventArgs> OnAnyEntityDodge;
    public event EventHandler<OnEntityDodgeEventArgs> OnEntityDodge;

    public static event EventHandler<OnEntityHealthTakeDamageEventArgs> OnAnyEntityHealthTakeDamage;
    public event EventHandler<OnEntityHealthTakeDamageEventArgs> OnEntityHealthTakeDamage;

    public static event EventHandler<OnEntityShieldTakeDamageEventArgs> OnAnyEntityShieldTakeDamage;
    public event EventHandler<OnEntityShieldTakeDamageEventArgs> OnEntityShieldTakeDamage;

    public static event EventHandler<OnEntityHealEventArgs> OnAnyEntityHeal;
    public event EventHandler<OnEntityHealEventArgs> OnEntityHeal;

    public static event EventHandler<OnEntityShieldRestoredEventArgs> OnAnyEntityShieldRestored;
    public event EventHandler<OnEntityShieldRestoredEventArgs> OnEntityShieldRestored;

    public static event EventHandler OnAnyEntityDeath;
    public event EventHandler OnEntityDeath;

    ///

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMaxHealthChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMaxHealthChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMaxShieldChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMaxShieldChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityArmorChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityArmorChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityDodgeChanceChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityDodgeChanceChanged;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityCurrentHealthClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityCurrentHealthClamped;
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityCurrentShieldClamped;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityCurrentShieldClamped;

    protected bool hasInitialized = false;

    #endregion

    #region EventArgs Classes
    public class OnEntityStatsEventArgs : EventArgs
    {
        public int maxHealth;
        public int currentHealth;

        public int maxShield;
        public int currentShield;

        public int armor;
        public float dodgeChance;
    }

    public class OnEntityDodgeEventArgs : EventArgs
    {
        public int damageDodged;
        public bool isCrit;

        public IDamageSourceSO damageSource;
    }

    public class OnEntityShieldTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByShield;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public bool isCrit;

        public IDamageSourceSO damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnEntityHealthTakeDamageEventArgs : EventArgs
    {
        public int damageTakenByHealth;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public bool isCrit;

        public IDamageSourceSO damageSource;
        public IHasHealth damageReceiver;
    }

    public class OnEntityHealEventArgs : EventArgs
    {
        public int healDone;

        public int previousHealth;
        public int newHealth;
        public int maxHealth;

        public IHealSourceSO healSource;
        public IHasHealth healReceiver;
    }

    public class OnEntityShieldRestoredEventArgs : EventArgs
    {
        public int shieldRestored;

        public int previousShield;
        public int newShield;
        public int maxShield;

        public IShieldSourceSO shieldSource;
        public IHasHealth shieldReceiver;
    }
    #endregion

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHealth = currentHealth <= 0 ? CalculateMaxHealth() : currentHealth;
        currentShield = currentShield <= 0 ? CalculateMaxShield() : currentShield;

        maxHealth = CalculateMaxHealth();
        maxShield = CalculateMaxShield();
        armor = CalculateArmor();
        dodgeChance = CalculateDodgeChance();

        OnEntityStatsInitializedMethod();
    }

    protected abstract int CalculateMaxHealth();
    protected abstract int CalculateMaxShield();
    protected abstract int CalculateArmor();
    protected abstract float CalculateDodgeChance();

    #region Recalculate Stats

    protected virtual void RecalculateMaxHealth()
    {
        maxHealth = CalculateMaxHealth();
        OnEntityMaxHealthChangedMethod();

        CheckCurrentHealthClamped();
    }

    protected virtual void RecalculateMaxShield()
    {
        maxShield = CalculateMaxShield();
        OnEntityMaxShieldChangedMethod();

        CheckCurrentShieldClamped();
    }

    protected virtual void RecalculateArmor()
    {
        armor = CalculateArmor();
        OnEntityArmorChangedMethod();
    }

    protected virtual void RecalculateDodgeChance()
    {
        dodgeChance = CalculateDodgeChance();
        OnEntityDodgeChanceChangedMethod();
    }
    #endregion

    #region Stats Clamping
    protected virtual void CheckCurrentHealthClamped()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            OnEntityCurrentHealthClampedMethod();
        }
    }

    protected virtual void CheckCurrentShieldClamped()
    {
        if(currentShield > maxShield)
        {
            currentShield = maxShield;
            OnEntityCurrentShieldClampedMethod();
        }
    }
    #endregion

    #region Interface Methods

    public virtual bool CanTakeDamage() => IsAlive();
    public virtual bool CanAvoidDamage() => false; //Technically not an interface method. Only for entities
    public virtual bool CanHeal() => IsAlive();
    public virtual bool CanRestoreShield() => IsAlive();

    public bool TakeDamage(DamageData damageData) 
    {
        if (!IsAlive()) return false;
        if(CanAvoidDamage()) return false;
        if(!CanTakeDamage()) return true;

        bool dodged = MechanicsUtilities.EvaluateDodgeChance(CalculateDodgeChance());

        if (dodged && damageData.canBeDodged)
        {
            OnEntityDodgeMethod(damageData);
            return false;
        }

        int armorMitigatedDamage = MechanicsUtilities.MitigateDamageByArmor(damageData.damage, CalculateArmor());

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield, damageTakenByHealth;

        if (HasShield())
        {
            damageTakenByShield = currentShield < armorMitigatedDamage ? currentShield : armorMitigatedDamage; //Shield Absorbs all Damage, Ex: if an entity has 3 Shield and would take 10 damage, it destroys all shield and health does not receive damage at all
            damageTakenByHealth = 0;
        }
        else
        {
            damageTakenByShield = 0;
            damageTakenByHealth = currentHealth < armorMitigatedDamage ? currentHealth : armorMitigatedDamage;
        }

        currentShield = currentShield < damageTakenByShield ? 0 : currentShield - damageTakenByShield;
        currentHealth = currentHealth < damageTakenByHealth ? 0 : currentHealth - damageTakenByHealth;

        if(damageTakenByShield > 0)
        {
            OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, damageData.isCrit, damageData.damageSource);
        }

        if(damageTakenByHealth > 0)
        {
            OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, damageData.isCrit, damageData.damageSource);
        }

        if (!IsAlive()) OnEntityDeathMethod();

        return true;
    }

    public void Excecute(IDamageSourceSO damageSource)
    {
        if (!CanTakeDamage()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;
        int previousShield = currentShield;

        int damageTakenByShield = HasShield()? MechanicsUtilities.GetExecuteDamage() : 0;
        int damageTakenByHealth = MechanicsUtilities.GetExecuteDamage();

        currentShield = 0;
        currentHealth = 0;
         
        if(damageTakenByShield > 0)
        {
            OnEntityShieldTakeDamageMethod(damageTakenByShield, previousShield, true, damageSource);
        }

        if(damageTakenByHealth > 0)
        {
            OnEntityHealthTakeDamageMethod(damageTakenByHealth, previousHealth, true, damageSource);
        }

        OnEntityDeathMethod();
    }

    public void Heal(HealData healData)
    {
        if (!CanHeal()) return;
        if(!IsAlive()) return;

        int previousHealth = currentHealth;

        int effectiveHealAmount = currentHealth + healData.healAmount > CalculateMaxHealth() ? CalculateMaxHealth() - currentHealth : healData.healAmount;
        currentHealth = currentHealth + effectiveHealAmount > CalculateMaxHealth()? CalculateMaxHealth() : currentHealth + effectiveHealAmount;

        OnEntityHealMethod(effectiveHealAmount, previousHealth, healData.healSource);
    }
    public void HealCompletely(IHealSourceSO healSource)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int healAmount = CalculateMaxHealth() - currentHealth;
        currentHealth = CalculateMaxHealth();

        OnEntityHealMethod(healAmount, previousHealth, healSource);
    }

    public void RestoreShield(ShieldData shieldData)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int effectiveShieldRestored = currentShield + shieldData.shieldAmount > CalculateMaxShield() ? CalculateMaxShield() - currentShield : shieldData.shieldAmount;
        currentShield = currentShield + effectiveShieldRestored > CalculateMaxShield() ? CalculateMaxShield() : currentShield + effectiveShieldRestored;

        OnEntityShieldRestoredMethod(effectiveShieldRestored, previousShield, shieldData.shieldSource);
    }

    public void RestoreShieldCompletely(IShieldSourceSO shieldSource)
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

    #region Virtual Event Methods
    
    protected virtual void OnEntityStatsInitializedMethod()
    {
        OnEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityStatsUpdatedMethod()
    {
        OnEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }


    protected virtual void OnEntityDodgeMethod(DamageData damageData)
    {
        OnEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected virtual void OnEntityHealthTakeDamageMethod(int damageTakenByHealth, int previousHealth, bool isCrit, IDamageSourceSO damageSource)
    {
        OnEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = CalculateMaxHealth(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        OnEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = CalculateMaxShield(), isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected virtual void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = CalculateMaxHealth(), healSource = healSource, healReceiver = this});
    }

    protected virtual void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        OnEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = CalculateMaxShield(), shieldSource = shieldSource, shieldReceiver = this });
    }

    protected virtual void OnEntityDeathMethod()
    {
        OnEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyEntityDeath?.Invoke(this, EventArgs.Empty);
    }

    //

    protected virtual void OnEntityMaxHealthChangedMethod()
    {
        OnEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityMaxShieldChangedMethod()
    {
        OnEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityArmorChangedMethod()
    {
        OnEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityDodgeChanceChangedMethod()
    {
        OnEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityCurrentHealthClampedMethod()
    {
        OnEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    protected virtual void OnEntityCurrentShieldClampedMethod()
    {
        OnEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});

        OnAnyEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = CalculateMaxHealth(), currentHealth = currentHealth, maxShield = CalculateMaxHealth(), currentShield = currentShield, 
        armor = CalculateArmor(), dodgeChance = CalculateDodgeChance()});
    }

    #endregion
}
