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
    [SerializeField] protected AbilitySlot abilitySlot;
    [SerializeField] protected AbilityLevel abilityLevel;
    [SerializeField] protected bool isActiveVariant;

    public AbilitySO AbilitySO => abilitySO;
    public AbilitySlot AbilitySlot => abilitySlot;
    public AbilityLevel AbilityLevel => abilityLevel;
    public bool IsActiveVariant => isActiveVariant;

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
        public AbilitySO abilitySO;
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

    protected virtual void Start()
    {
        AssignAbilitySlot();
    }

    protected virtual void Update()
    {
        HandleAbilityCasting();
        HandleUpdateLogic();
    }

    protected virtual void FixedUpdate()
    {
        HandleFixedUpdateLogic();
    }

    private void AssignAbilitySlot()
    {
        abilitySlot = abilitySlotHandler.AbilitySlot;
    }

    //All abilities can be cast. If only passive, cast is always denied
    protected virtual void HandleAbilityCasting()
    {
        if (!isActiveVariant) return; //Cut logic from the start if not Active Variant

        if (!GetAssociatedDownInput()) return;

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
        OnAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnAnyAbilityCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
    }

    protected virtual void OnAbilityCastDeniedMethod()
    {
        OnAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnAnyAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
    }

    public virtual bool CanCastAbility()
    {
        if (abilitySO.GetAbilityType() == AbilityType.Passive) return false; //Can not cast if only passive
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;

        return true;
    }
    #endregion

    #region Input Association
    protected bool GetAssociatedDownInput()
    {
        switch (abilitySlot)
        {
            case AbilitySlot.Passive:
            default:
                return false;
            case AbilitySlot.AbilityA:
                return AbilitiesInput.Instance.GetAbilityADown();
            case AbilitySlot.AbilityB:
                return AbilitiesInput.Instance.GetAbilityBDown();
            case AbilitySlot.AbilityC:
                return AbilitiesInput.Instance.GetAbilityCDown();
        }
    }

    protected bool GetAsociatedHoldInput()
    {
        switch (abilitySlot)
        {
            case AbilitySlot.Passive:
            default:
                return false;
            case AbilitySlot.AbilityA:
                return AbilitiesInput.Instance.GetAbilityAHold();
            case AbilitySlot.AbilityB:
                return AbilitiesInput.Instance.GetAbilityBHold();
            case AbilitySlot.AbilityC:
                return AbilitiesInput.Instance.GetAbilityCHold();
        }
    }
    #endregion

    #region Subscriptions
    private void PlayerAbilitySlotsVariantsHandler_OnAbilityVariantInitialized(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (!isActiveVariant && e.newAbilityVariant == this)
        {
            isActiveVariant = true;
            OnAbilityVariantActivationMethod();
        }
        else if (isActiveVariant && e.previousAbilityVariant == this)
        {
            isActiveVariant = false;
            OnAbilityVariantDeactivationMethod();
        }
    }

    private void AbilityVariantHandler_OnAbilityVariantSelected(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (!isActiveVariant && e.newAbilityVariant == this)
        {
            isActiveVariant = true;
            OnAbilityVariantActivationMethod();
        }
        else if (isActiveVariant && e.previousAbilityVariant == this)
        {
            isActiveVariant = false;
            OnAbilityVariantDeactivationMethod();
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
