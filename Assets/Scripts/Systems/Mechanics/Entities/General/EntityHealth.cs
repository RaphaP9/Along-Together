using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IHasHealth
{
    [Header("EntityHealth Components")]
    [SerializeField] private List<Transform> dodgeAbiltiesTransforms;
    [SerializeField] private List<Transform> immuneAbiltiesTransforms;

    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;
    [Space]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxShield;
    [SerializeField] protected int armor;
    [SerializeField] protected float dodgeChance;

    private List<IDodgeAbility> dodgeAbilties;
    private List<IImmuneAbility> immuneAbilities;

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

    public static event EventHandler<OnEntityImmuneEventArgs> OnAnyEntityImmune;
    public event EventHandler<OnEntityImmuneEventArgs> OnEntityImmune;

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

    public class OnEntityImmuneEventArgs : EventArgs
    {
        public int damageImmuned;
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

    protected virtual void Awake()
    {
        GetDodgeAbilitiesInterfaces();
        GetImmuneAbilitiesInterfaces();
    }

    protected virtual void Start()
    {
        Initialize();
    }

    private void GetDodgeAbilitiesInterfaces() => dodgeAbilties = GeneralUtilities.TryGetGenericsFromTransforms<IDodgeAbility>(dodgeAbiltiesTransforms);
    private void GetImmuneAbilitiesInterfaces() => immuneAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IImmuneAbility>(immuneAbiltiesTransforms);

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
    public virtual bool CanHeal() => IsAlive();
    public virtual bool CanRestoreShield() => IsAlive();

    public bool TakeDamage(DamageData damageData) 
    {
        if(!CanTakeDamage()) return false;

        if(IsImmuneByAbility() && damageData.canBeImmuned)
        {
            OnEntityImmuneMethod(damageData);
            return true;
        }

        bool dodged = MechanicsUtilities.EvaluateDodgeChance(CalculateDodgeChance());

        if ((dodged||IsDodgingByAbility()) && damageData.canBeDodged)
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

        int effectiveHealAmount = currentHealth + healData.healAmount > maxHealth ? maxHealth - currentHealth : healData.healAmount;
        currentHealth = currentHealth + effectiveHealAmount > maxHealth ? maxHealth : currentHealth + effectiveHealAmount;

        OnEntityHealMethod(effectiveHealAmount, previousHealth, healData.healSource);
    }
    public void HealCompletely(IHealSourceSO healSource)
    {
        if (!CanHeal()) return;
        if (!IsAlive()) return;

        int previousHealth = currentHealth;

        int healAmount = maxHealth - currentHealth;
        currentHealth = maxHealth;

        OnEntityHealMethod(healAmount, previousHealth, healSource);
    }

    public void RestoreShield(ShieldData shieldData)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int effectiveShieldRestored = currentShield + shieldData.shieldAmount > maxShield ? maxShield - currentShield : shieldData.shieldAmount;
        currentShield = currentShield + effectiveShieldRestored > maxShield ? maxShield : currentShield + effectiveShieldRestored;

        OnEntityShieldRestoredMethod(effectiveShieldRestored, previousShield, shieldData.shieldSource);
    }

    public void RestoreShieldCompletely(IShieldSourceSO shieldSource)
    {
        if (!CanRestoreShield()) return;
        if (!IsAlive()) return;

        int previousShield = currentShield;

        int shieldAmount = maxShield - currentShield;
        currentShield = maxShield;

        OnEntityShieldRestoredMethod(shieldAmount, previousShield, shieldSource);
    }

    public bool IsFullHealth() => currentHealth >= maxHealth;
    public bool IsFullShield() => currentShield >= maxShield;
    public bool IsAlive() => currentHealth > 0;
    public bool HasShield() => currentShield > 0;

    #endregion

    #region Virtual Event Methods
    
    protected virtual void OnEntityStatsInitializedMethod()
    {
        OnEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityStatsUpdatedMethod()
    {
        OnEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }


    protected virtual void OnEntityDodgeMethod(DamageData damageData)
    {
        OnEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEntityDodge?.Invoke(this, new OnEntityDodgeEventArgs { damageDodged = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
    }

    protected virtual void OnEntityImmuneMethod(DamageData damageData)
    {
        OnEntityImmune?.Invoke(this, new OnEntityImmuneEventArgs { damageImmuned = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
        OnAnyEntityImmune?.Invoke(this, new OnEntityImmuneEventArgs { damageImmuned = damageData.damage, isCrit = damageData.isCrit, damageSource = damageData.damageSource });
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
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxHealth, healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = MaxHealth, healSource = healSource, healReceiver = this});
    }

    protected virtual void OnEntityShieldRestoredMethod(int shieldAmount, int previousShield, IShieldSourceSO shieldSource)
    {
        OnEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
        OnAnyEntityShieldRestored?.Invoke(this, new OnEntityShieldRestoredEventArgs { shieldRestored = shieldAmount, previousShield = previousShield, newShield = currentShield, maxShield = maxShield, shieldSource = shieldSource, shieldReceiver = this });
    }

    protected virtual void OnEntityDeathMethod()
    {
        OnEntityDeath?.Invoke(this, EventArgs.Empty);
        OnAnyEntityDeath?.Invoke(this, EventArgs.Empty);
    }

    //

    protected virtual void OnEntityMaxHealthChangedMethod()
    {
        OnEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityMaxHealthChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityMaxShieldChangedMethod()
    {
        OnEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityMaxShieldChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityArmorChangedMethod()
    {
        OnEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityArmorChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityDodgeChanceChangedMethod()
    {
        OnEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityDodgeChanceChanged?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityCurrentHealthClampedMethod()
    {
        OnEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityCurrentHealthClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    protected virtual void OnEntityCurrentShieldClampedMethod()
    {
        OnEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});

        OnAnyEntityCurrentShieldClamped?.Invoke(this, new OnEntityStatsEventArgs { maxHealth = maxHealth, currentHealth = currentHealth, maxShield = maxShield, currentShield = currentShield, 
        armor = armor, dodgeChance = dodgeChance});
    }

    #endregion

    #region Abilities Methods

    private bool IsDodgingByAbility()
    {
        if (!IsAlive()) return false;

        foreach (IDodgeAbility dodgeAbility in dodgeAbilties)
        {
            if (dodgeAbility.IsDodging()) return true;
        }

        return false;
    }

    private bool IsImmuneByAbility()
    {
        if (!IsAlive()) return false;

        foreach (IImmuneAbility immuneAbility in immuneAbilities)
        {
            if (immuneAbility.IsImmune()) return true;
        }

        return false;
    }
    #endregion
}
