using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected AbilitySO abilitySO;
    [SerializeField] protected AbilityVariantHandler abilityVariantHandler;
    [SerializeField] protected PlayerHealth playerHealth;

    [Header("Runtime Filled")]
    [SerializeField] protected AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilitySO;
    private AbilityLevel AbilityLevel => abilityLevel;

    protected bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;

    public static event EventHandler<OnAbilityCastEventArgs> OnAbilityCastDenied;
    public static event EventHandler<OnAbilityCastEventArgs> OnAbilityCast;

    public class OnAbilityLevelIncreaseEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel newAbilityLevel;
    }

    public class OnAbilityCastEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
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
    }

    protected virtual void OnAbilityCastDeniedMethod()
    {
        OnAbilityCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
    }

    public virtual bool CanCastAbility()
    {
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;

        return true;
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
}
