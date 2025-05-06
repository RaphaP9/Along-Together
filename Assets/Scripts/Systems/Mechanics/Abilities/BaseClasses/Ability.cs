using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySO abilitySO;

    [Header("Runtime Filled")]
    [SerializeField] private AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilitySO;
    private AbilityLevel AbilityLevel => abilityLevel;

    protected bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;

    public class OnAbilityLevelIncreaseEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel newAbilityLevel;
    }

    public class OnAbilityCastEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
    }

    #region Input Association
    protected bool GetAssociatedDownInput()
    {
        switch (abilitySO.abilitySlot)
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
        switch (abilitySO.abilitySlot)
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
