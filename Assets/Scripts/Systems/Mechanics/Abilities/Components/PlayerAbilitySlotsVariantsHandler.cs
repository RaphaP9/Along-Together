using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitySlotsVariantsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilitySlotGroup> abilitySlotGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilitySlotGroup> AbilitySlotGroups => abilitySlotGroups;

    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantInitialized;
    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantSelected;

    private const AbilitySlot DEFAULT_ABILITY_SLOT = AbilitySlot.Unasigned;

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
        InitializeAbilityVariants();
    }

    private void InitializeAbilityVariants()
    {
        foreach(AbilitySlotGroup slotAbilityVariant in abilitySlotGroups)
        {
            InitializeAbilityVariant(slotAbilityVariant.startingAbilityVariant);
        }
    }

    private void InitializeAbilityVariant(Ability abilityVariant)
    {
        AbilitySlotGroup abilitySlotGroup = GetAbilitySlotGroupByAbilityOnIt(abilityVariant);

        if (abilitySlotGroup == null)
        {
            if (debug) Debug.Log("Ability Slot Group is null. Selection will be ignored.");
            return;
        }

        Ability previousAbilityVariant = null;
        abilitySlotGroup.selectedAbilityVariant = abilityVariant;

        OnAbilityVariantInitialized?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlotGroup = abilitySlotGroup, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = abilitySlotGroup.selectedAbilityVariant });
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

    #region Set & Get

    public void SetStartingAbilityVariants(List<PrimitiveAbilitySlotGroup> setterPrimitiveAbilitySlotGroups) //To be called By the GameplaySessionDataSaveLoader, before Start()
    {
        foreach (PrimitiveAbilitySlotGroup setterPrimitiveAbilitySlotGroup in setterPrimitiveAbilitySlotGroups)
        {
            foreach (AbilitySlotGroup abilitySlotGroup in abilitySlotGroups)
            {
                if (abilitySlotGroup.abilitySlot != setterPrimitiveAbilitySlotGroup.abilitySlot) continue;

                foreach(Ability abilityVariant in abilitySlotGroup.abilityVariants)
                {
                    if(abilityVariant.AbilitySO == setterPrimitiveAbilitySlotGroup.abilitySO)
                    {
                        abilitySlotGroup.startingAbilityVariant = abilityVariant;
                        break;
                    }
                }

                if (debug) Debug.Log($"Ability with name: {setterPrimitiveAbilitySlotGroup.abilitySO.abilityName} is not found in Slot: {setterPrimitiveAbilitySlotGroup.abilitySlot}");
            }
        }
    }

    public List<PrimitiveAbilitySlotGroup> GetPrimitiveAbilitySlotGroups()
    {
        List<PrimitiveAbilitySlotGroup> primitiveAbilitySlotGroups = new List<PrimitiveAbilitySlotGroup>();

        foreach (AbilitySlotGroup abilitySlotGroup in abilitySlotGroups)
        {
            PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup = new PrimitiveAbilitySlotGroup { abilitySlot = abilitySlotGroup.abilitySlot, abilitySO = abilitySlotGroup.selectedAbilityVariant.AbilitySO};
            primitiveAbilitySlotGroups.Add(primitiveAbilitySlotGroup);
        }

        return primitiveAbilitySlotGroups;
    }
    #endregion
}
