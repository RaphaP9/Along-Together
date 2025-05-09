using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlotHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AbilitySlot abilitySlot;
    [SerializeField] private List<Ability> abilityVariants;

    [SerializeField] private Ability startingAbilityVariant;

    [Header("Runtime Filled")]
    [SerializeField] private Ability selectedAbilityVariant;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public AbilitySlot AbilitySlot => abilitySlot;

    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantInitialized;
    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantSelected;

    public class OnAbilityVariantSelectionEventArgs : EventArgs
    {
        public AbilitySlot abilitySlot;
        public Ability previousAbilityVariant;
        public Ability newAbilityVariant;
    }

    private void Start()
    {
        InitializeAbilityVariant(startingAbilityVariant);
    }

    private void InitializeAbilityVariant(Ability abilityVariant)
    {
        Ability previousAbilityVariant = null;
        selectedAbilityVariant = abilityVariant;

        OnAbilityVariantInitialized?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlot = abilitySlot, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = selectedAbilityVariant });
    }

    private void SelectAbilityVariant(Ability abilityVariant)
    {
        Ability previousAbilityVariant = selectedAbilityVariant;
        selectedAbilityVariant = abilityVariant;

        OnAbilityVariantSelected?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlot = abilitySlot, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = selectedAbilityVariant });
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
        foreach (Ability abilityVariant in abilityVariants)
        {
            if (abilityVariant.AbilitySO == abilitySO) return abilityVariant;
        }

        if (debug) Debug.Log($"Could not find Ability Variant for AbilitySO with name : {abilitySO.abilityName} in Slot: {abilitySlot}. Returning null.");
        return null;
    }
    #endregion

    #region Get & Set
    public void SetStartingAbilityVariant(PrimitiveAbilitySlotGroup setterPrimitiveAbilitySlotGroup)
    {
        Ability ability = GetAbilityByAbilitySO(setterPrimitiveAbilitySlotGroup.abilitySO);

        if (ability == null)
        {
            if (debug) Debug.Log($"Ability with name: {setterPrimitiveAbilitySlotGroup.abilitySO.abilityName} is not found in Slot: {abilitySlot}");
            return;
        }

        startingAbilityVariant = ability;
    }

    public PrimitiveAbilitySlotGroup GetPrimitiveAbilitySlotGroup()
    {
        PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup = new PrimitiveAbilitySlotGroup { abilitySlot = abilitySlot, abilitySO = selectedAbilityVariant.AbilitySO };
        return primitiveAbilitySlotGroup;
    }
    #endregion
}
