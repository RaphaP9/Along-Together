using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IHasHealth
{
    [Header("Entity Health Components")]
    [SerializeField] protected SpecificEntityStatsResolver specificEntityStatsResolver;
    [SerializeField] protected List<Transform> dodgeAbiltiesTransforms;
    [SerializeField] protected List<Transform> immuneAbiltiesTransforms;

    [Header("Runtime Filled")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int currentShield;
    [Space]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxShield;
    [SerializeField] protected int armor;
    [SerializeField] protected float dodgeChance;

    protected List<IDodgeAbility> dodgeAbilties;
    protected List<IImmuneAbility> immuneAbilities;

    #region Properties
    public int CurrentHealth => currentHealth;
    public int CurrentShield => currentShield;
    public int MaxHealth => maxHealth;
    public int MaxShield => maxShield;
    public int Armor => armor;
    public float DodgeChance => dodgeChance;
    #endregion

    #region Events
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

    public static event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnAnyEntityCurrentHealthClamped;
    public event EventHandler<OnEntityCurrentHealthClampedEventArgs> OnEntityCurrentHealthClamped;

    public static event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnAnyEntityCurrentShieldClamped;
    public event EventHandler<OnEntityCurrentShieldClampedEventArgs> OnEntityCurrentShieldClamped;

    #endregion

    #region EventArgs Classes
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

    public class OnEntityCurrentHealthClampedEventArgs: EventArgs
    {
        public int currentHealth;
        public int maxHealth;
    }

    public class OnEntityCurrentShieldClampedEventArgs : EventArgs
    {
        public int currentShield;
        public int maxShield;
    }
    #endregion

    protected virtual void OnEnable()
    {
        specificEntityStatsResolver.OnEntityMaxHealthChanged += SpecificEntityStatsResolver_OnEntityMaxHealthChanged;
        specificEntityStatsResolver.OnEntityMaxShieldChanged += SpecificEntityStatsResolver_OnEntityMaxShieldChanged;
    }

    protected virtual void OnDisable()
    {
        specificEntityStatsResolver.OnEntityMaxHealthChanged -= SpecificEntityStatsResolver_OnEntityMaxHealthChanged;
        specificEntityStatsResolver.OnEntityMaxShieldChanged -= SpecificEntityStatsResolver_OnEntityMaxShieldChanged;
    }

    protected virtual void Awake()
    {
        GetDodgeAbilitiesInterfaces();
        GetImmuneAbilitiesInterfaces();
    }

    private void GetDodgeAbilitiesInterfaces() => dodgeAbilties = GeneralUtilities.TryGetGenericsFromTransforms<IDodgeAbility>(dodgeAbiltiesTransforms);
    private void GetImmuneAbilitiesInterfaces() => immuneAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IImmuneAbility>(immuneAbiltiesTransforms);

  
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

        bool dodged = MechanicsUtilities.EvaluateDodgeChance(specificEntityStatsResolver.DodgeChance);

        if ((dodged||IsDodgingByAbility()) && damageData.canBeDodged)
        {
            OnEntityDodgeMethod(damageData);
            return false;
        }

        int armorMitigatedDamage = MechanicsUtilities.MitigateDamageByArmor(damageData.damage, specificEntityStatsResolver.Armor);

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
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityHealthTakeDamage?.Invoke(this, new OnEntityHealthTakeDamageEventArgs {damageTakenByHealth = damageTakenByHealth, previousHealth = previousHealth, 
        newHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});
    }

    protected virtual void OnEntityShieldTakeDamageMethod(int damageTakenByShield, int previousShield, bool isCrit, IDamageSourceSO damageSource)
    {
        OnEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

        OnAnyEntityShieldTakeDamage?.Invoke(this, new OnEntityShieldTakeDamageEventArgs {damageTakenByShield = damageTakenByShield, previousShield = previousShield, 
        newShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield, isCrit = isCrit, damageSource = damageSource, damageReceiver = this});

    }

    protected virtual void OnEntityHealMethod(int healAmount, int previousHealth, IHealSourceSO healSource)
    {
        OnEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxHealth, healSource = healSource, healReceiver = this});
        OnAnyEntityHeal?.Invoke(this, new OnEntityHealEventArgs { healDone = healAmount, previousHealth = previousHealth, newHealth = currentHealth, maxHealth = maxHealth, healSource = healSource, healReceiver = this});
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

    protected virtual void OnEntityCurrentHealthClampedMethod()
    {
        OnEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth });
        OnAnyEntityCurrentHealthClamped?.Invoke(this, new OnEntityCurrentHealthClampedEventArgs { currentHealth = currentHealth, maxHealth = specificEntityStatsResolver.MaxHealth });
    }

    protected virtual void OnEntityCurrentShieldClampedMethod()
    {
        OnEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
        OnAnyEntityCurrentShieldClamped?.Invoke(this, new OnEntityCurrentShieldClampedEventArgs { currentShield = currentShield, maxShield = specificEntityStatsResolver.MaxShield });
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

    #region Subscriptions
    private void SpecificEntityStatsResolver_OnEntityMaxHealthChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        CheckCurrentHealthClamped();
    }

    private void SpecificEntityStatsResolver_OnEntityMaxShieldChanged(object sender, SpecificEntityStatsResolver.OnEntityStatsEventArgs e)
    {
        CheckCurrentShieldClamped();
    }
    #endregion
}
