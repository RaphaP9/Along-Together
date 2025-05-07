using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AbilitySlotsVariantsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilitySlotGroup> abilitySlotGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilitySlotGroup> AbilitySlotGroups => abilitySlotGroups;

    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantSelected;

    private const AbilitySlot DEFAULT_ABILITY_SLOT = AbilitySlot.Passive;

    [Serializable]
    public class AbilitySlotGroup
    {
        public AbilitySlot abilitySlot;
        public List<Ability> abilityVariants;

        public Ability startingAbilityVariant;

        [Header("Runtime Filled")]
        public Ability selectedAbilityVariant;
    }

    public class OnAbilityVariantSelectionEventArgs : EventArgs
    {
        public AbilitySlotGroup abilitySlotGroup;
        public Ability previousAbilityVariant;
        public Ability newAbilityVariant;
    }

    private void Start()
    {
        SelectStartingAbilityVariants();
    }

    private void SelectStartingAbilityVariants()
    {
        foreach(AbilitySlotGroup slotAbilityVariant in abilitySlotGroups)
        {
            SelectAbilityVariant(slotAbilityVariant.startingAbilityVariant);
        }
    }

    private void SelectAbilityVariant(Ability abilityVariant)
    {
        AbilitySlotGroup abilitySlotGroup = GetAbilitySlotGroupByAbilityOnIt(abilityVariant);   

        if(abilitySlotGroup == null)
        {
            if (debug) Debug.Log("Ability Slot Group is null. Selection will be ignored.");
            return;
        }

        Ability previousAbilityVariant = abilitySlotGroup.selectedAbilityVariant;
        abilitySlotGroup.selectedAbilityVariant = abilityVariant;

        OnAbilityVariantSelected?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlotGroup = abilitySlotGroup, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = abilitySlotGroup.selectedAbilityVariant});
    }

    private void SelectAbilityVariantBySO(AbilitySO abilitySO)
    {
        Ability abilityVariant = GetAbilityByAbilitySO(abilitySO);

        if (abilityVariant == null)
        {
            if (debug) Debug.Log("Ability Variant is null. Selection will be ignored");
            return;
        }

        SelectAbilityVariant(abilityVariant);
    }

    #region Seekers

    private Ability GetAbilityByAbilitySO(AbilitySO abilitySO)
    {
        foreach(AbilitySlotGroup abilitySlotGroup in abilitySlotGroups)
        {
            foreach(Ability abilityVariant in abilitySlotGroup.abilityVariants)
            {
                if (abilityVariant.AbilitySO == abilitySO) return abilityVariant;
            }
        }

        if (debug) Debug.Log($"Could not find Ability Variant for AbilitySO with name : {abilitySO.abilityName}. Returning null.");
        return null;
    }

    private AbilitySlotGroup GetAbilitySlotGroupByAbilitySlot(AbilitySlot abilitySlot)
    {
        foreach(AbilitySlotGroup abilitySlotGroup in abilitySlotGroups)
        {
            if(abilitySlotGroup.abilitySlot == abilitySlot) return abilitySlotGroup;
        }

        if (debug) Debug.Log($"Could not find SlotGroup with abilitySlot: {abilitySlot}. Returning null.");
        return null;
    }

    private AbilitySlotGroup GetAbilitySlotGroupByAbilityOnIt(Ability ability)
    {
        foreach (AbilitySlotGroup abilitySlotGroup in abilitySlotGroups)
        {
            foreach(Ability abilityVariant in abilitySlotGroup.abilityVariants)
            {
                if(abilityVariant == ability) return abilitySlotGroup;
            }
        }

        if (debug) Debug.Log($"Could not find SlotGroup with Ability: {ability.AbilitySO.abilityName}. Returning null.");
        return null;
    }

    public AbilitySlot GetAbilitySlot(Ability ability)
    {
        AbilitySlotGroup abilitySlotGroup = GetAbilitySlotGroupByAbilityOnIt(ability);

        if (abilitySlotGroup == null)
        {
            if (debug) Debug.Log($"Could not find AbilitySlot Assigned for Ability: {ability.AbilitySO.abilityName}. Returning default ability slot value.");
            return DEFAULT_ABILITY_SLOT;
        }

        return abilitySlotGroup.abilitySlot;
    }
    #endregion
}
