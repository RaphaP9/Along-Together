using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected AbilitySO abilitySO;
    [SerializeField] protected AbilityVariantsHandler abilityVariantHandler;
    [SerializeField] protected PlayerHealth playerHealth;

    [Header("Runtime Filled")]
    [SerializeField] protected AbilityLevel abilityLevel;
    [SerializeField] protected bool isActiveVariant;

    public AbilitySO AbilitySO => abilitySO;
    private AbilityLevel AbilityLevel => abilityLevel;

    protected bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;

    #region Events

    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCastDenied;
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyAbilityCast;

    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCastDenied;
    public event EventHandler<OnAbilityCastEventArgs> OnAbilityCast;

    public static event EventHandler<OnAbilityLevelIncreaseEventArgs> OnAnyAbilityLevelIncreased;
    public event EventHandler<OnAbilityLevelIncreaseEventArgs> OnAbilityLevelIncreased;

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
        abilityVariantHandler.OnAbilityVariantSelected += AbilityVariantHandler_OnAbilityVariantSelected;
    }

    protected virtual void OnDisable()
    {
        abilityVariantHandler.OnAbilityVariantSelected -= AbilityVariantHandler_OnAbilityVariantSelected;
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

    protected virtual void HandleAbilityCasting()
    {
        if (!isActiveVariant) return; //Can not cast if not Active Variant
        if (abilitySO.abilityType == AbilityType.Passive) return; //Can not cast if only passive

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

    #region Abstract Methods
    protected abstract void HandleUpdateLogic();
    protected abstract void HandleFixedUpdateLogic();

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
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;

        return true;
    }

    protected virtual void ActivateAbilityVariant()
    {
        isActiveVariant = true;
    }
    protected virtual void DisableAbilityVariant()
    {
        isActiveVariant = false;
    }

    #endregion

    #region Input Association
    protected bool GetAssociatedDownInput()
    {
        switch (abilityVariantHandler.AbilitySlot)
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
        switch (abilityVariantHandler.AbilitySlot)
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
    private void AbilityVariantHandler_OnAbilityVariantSelected(object sender, AbilityVariantsHandler.OnAbilityVariantSelectionEventArgs e)
    {
        if (!isActiveVariant && e.newAbilityVariant == this)
        {
            ActivateAbilityVariant();
        }
        else if(isActiveVariant && e.previousAbilityVariant == this)
        {
            DisableAbilityVariant();
        }
    }
    #endregion
}
