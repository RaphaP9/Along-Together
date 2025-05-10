using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Ability Components")]
    [SerializeField] protected AbilitySO abilitySO;
    [Space]
    [SerializeField] protected AbilitySlotHandler abilitySlotHandler;
    [SerializeField] protected PlayerAbilityLevelsHandler playerAbilityLevelsHandler;
    [SerializeField] protected PlayerHealth playerHealth;

    [Header("Ability Runtime Filled")]
    [SerializeField] protected AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilitySO;
    public AbilitySlot AbilitySlot => abilitySlotHandler.AbilitySlot;
    public AbilityLevel AbilityLevel => abilityLevel;
    public bool IsActiveVariant => abilitySlotHandler.ActiveAbilityVariant == this;

    #region Events

    //For specific Ability Casts, Sending the AbilitySO As Parammeter. Therefore check abilityID.
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCastDenied; 
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCast;

    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCastDenied;
    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCast;

    #endregion

    #region EventArgs Classes

    public class OnAbilityLevelIncreaseEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel newAbilityLevel;
    }

    public class OnAbilityCastEventArgs : EventArgs
    {
        public Ability ability;
    }

    #endregion

    protected virtual void OnEnable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized += PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected += AbilityVariantHandler_OnAbilityVariantSelected;

        playerAbilityLevelsHandler.OnAbilityLevelInitialized += PlayerAbilityLevelsHandler_OnAbilityLevelInitialized;
        playerAbilityLevelsHandler.OnAbilityLevelChanged += AbilityLevelsHandler_OnAbilityLevelChanged;
    }

    protected virtual void OnDisable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized -= PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected -= AbilityVariantHandler_OnAbilityVariantSelected;

        playerAbilityLevelsHandler.OnAbilityLevelInitialized -= PlayerAbilityLevelsHandler_OnAbilityLevelInitialized;
        playerAbilityLevelsHandler.OnAbilityLevelChanged -= AbilityLevelsHandler_OnAbilityLevelChanged;
    }

    protected virtual void Update()
    {
        HandleUpdateLogic();
    }

    protected virtual void FixedUpdate()
    {
        HandleFixedUpdateLogic();
    }

    //All abilities can be cast. If only passive, cast is always denied

    public void TryCastAbility()
    {
        if (CanCastAbility())
        {
            OnAbilityCastMethod();
        }
        else
        {
            OnAbilityCastDeniedMethod();
        }
    }

    protected bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;

    #region Abstract Methods
    protected abstract void HandleUpdateLogic();
    protected abstract void HandleFixedUpdateLogic();
    protected abstract void OnAbilityVariantActivationMethod();
    protected abstract void OnAbilityVariantDeactivationMethod();

    protected virtual void OnAbilityCastMethod()
    {
        OnAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
        OnAnyAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
    }

    protected virtual void OnAbilityCastDeniedMethod()
    {
        OnAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
        OnAnyAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { ability = this });
    }

    public virtual bool CanCastAbility()
    {
        if (abilitySO.GetAbilityType() == AbilityType.Passive) return false; //Can not cast if only passive
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;

        return true;
    }
    #endregion


    #region Subscriptions
    private void PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (e.previousAbilityVariant == this)
        {
            OnAbilityVariantDeactivationMethod();
        }

        if (e.newAbilityVariant == this)
        {
            OnAbilityVariantActivationMethod();
        }      
    }

    private void AbilityVariantHandler_OnAbilityVariantSelected(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (e.previousAbilityVariant == this)
        {
            OnAbilityVariantDeactivationMethod();
        }

        if (e.newAbilityVariant == this)
        {
            OnAbilityVariantActivationMethod();
        }
    }

    private void AbilityLevelsHandler_OnAbilityLevelChanged(object sender, PlayerAbilityLevelsHandler.OnAbilityLevelChangedEventArgs e)
    {
        if (e.abilityLevelGroup.ability != this) return;

        abilityLevel = e.newAbilityLevel;
    }

    private void PlayerAbilityLevelsHandler_OnAbilityLevelInitialized(object sender, PlayerAbilityLevelsHandler.OnAbilityLevelChangedEventArgs e)
    {
        if (e.abilityLevelGroup.ability != this) return;

        abilityLevel = e.newAbilityLevel;
    }

    #endregion
}
