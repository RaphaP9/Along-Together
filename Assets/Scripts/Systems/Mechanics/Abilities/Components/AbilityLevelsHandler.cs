using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AbilitySlotsVariantsHandler;
public class AbilityLevelsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilityLevelGroup> abilityLevelGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilityLevelGroup> AbilityLevelGroups => abilityLevelGroups;

    private const AbilityLevel DEFAULT_ABILITY_LEVEL = AbilityLevel.NotLearned;

    public event EventHandler<OnAbilityLevelChangedEventArgs> OnAbilityLevelChanged;

    public class OnAbilityLevelChangedEventArgs : EventArgs
    {
        public AbilityLevelGroup abilityLevelGroup;
        public AbilityLevel previousAbilityLevel;
        public AbilityLevel newAbilityLevel;
    }

    [Serializable]
    public class AbilityLevelGroup
    {
        public Ability ability;

        public AbilityLevel startingAbilityLevel;

        [Header("Runtime Filled")]
        public AbilityLevel currentAbilityLevel;
    }

    private void Start()
    {
        ChangeStartingAbilityLevels();
    }

    private void ChangeStartingAbilityLevels()
    {
        foreach (AbilityLevelGroup abilityLevelGroup in abilityLevelGroups)
        {
            ChangeAbilityLevel(abilityLevelGroup.ability, abilityLevelGroup.startingAbilityLevel);
        }
    }

    private void ChangeAbilityLevel(Ability ability, AbilityLevel abilityLevel)
    {
        AbilityLevelGroup abilityLevelGroup = GetAbilitySlotGroupByAbility(ability);

        if (abilityLevelGroup == null)
        {
            if (debug) Debug.Log("Ability Level Group is null. Selection will be ignored.");
            return;
        }

        AbilityLevel previousAbilityLevel = abilityLevelGroup.currentAbilityLevel;
        abilityLevelGroup.currentAbilityLevel = abilityLevel;

        OnAbilityLevelChanged?.Invoke(this, new OnAbilityLevelChangedEventArgs { abilityLevelGroup = abilityLevelGroup, previousAbilityLevel = previousAbilityLevel, newAbilityLevel = abilityLevelGroup.currentAbilityLevel });
    }

    private void ChangeAbilityLevelByAbilitySO(AbilitySO abilitySO, AbilityLevel abilityLevel)
    {
        Ability ability = GetAbilityByAbilitySO(abilitySO);

        if (ability == null)
        {
            if (debug) Debug.Log("Ability is null. Change will be ignored");
            return;
        }

        ChangeAbilityLevel(ability, abilityLevel);
    }

    #region Seekers
    private Ability GetAbilityByAbilitySO(AbilitySO abilitySO)
    {
        foreach (AbilityLevelGroup abilityLevelGroup in abilityLevelGroups)
        {
            if (abilityLevelGroup.ability.AbilitySO == abilitySO) return abilityLevelGroup.ability;
        }

        if (debug) Debug.Log($"Could not find Ability Variant for AbilitySO with name : {abilitySO.abilityName}. Returning null.");
        return null;
    }

    private AbilityLevelGroup GetAbilitySlotGroupByAbility(Ability ability)
    {
        foreach (AbilityLevelGroup abilityLevelGroup in abilityLevelGroups)
        {
            if (abilityLevelGroup.ability == ability) return abilityLevelGroup;
        }

        if (debug) Debug.Log($"Could not find SlotGroup with Ability: {ability.AbilitySO.abilityName}. Returning null.");
        return null;
    }

    public AbilityLevel GetAbilityLevel(Ability ability)
    {
        foreach(AbilityLevelGroup abilityLevelGroup in abilityLevelGroups)
        {
            if (abilityLevelGroup.ability == ability) return abilityLevelGroup.currentAbilityLevel;
        }

        if (debug) Debug.Log($"Could not find AbilityLevelGroup with Ability: {ability.AbilitySO.abilityName}. Returning default ability level value.");
        return DEFAULT_ABILITY_LEVEL;
    }
    #endregion
}
