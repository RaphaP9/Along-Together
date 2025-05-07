using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityVariantsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<Ability> abilityVariants;

    [Header("Settings")]
    [SerializeField] private AbilitySlot abilitySlot;
    [SerializeField] private Ability startingAbilityVariant;

    [Header("Runtime Filled")]
    [SerializeField] private Ability selectedAbilityVariant;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<Ability> AbilityVariants => abilityVariants;
    public Ability SelectedAbilityVariant => selectedAbilityVariant;
    public AbilitySlot AbilitySlot => abilitySlot;

    public event EventHandler<OnAbilityVariantSelectionEventArgs> OnAbilityVariantSelected;

    public class OnAbilityVariantSelectionEventArgs : EventArgs
    {
        public Ability previousAbilityVariant;
        public Ability newAbilityVariant;
        public AbilitySlot abilitySlot;
    }

    private void Start()
    {
        SelectAbiltiyVariant(startingAbilityVariant);
    }

    private void SelectAbiltiyVariant(Ability abilityVariant)
    {
        Ability previousAbilityVariant = selectedAbilityVariant;
        selectedAbilityVariant = abilityVariant;

        OnAbilityVariantSelected?.Invoke(this, new OnAbilityVariantSelectionEventArgs { previousAbilityVariant = previousAbilityVariant, newAbilityVariant = selectedAbilityVariant });
    }

    private void SelectAbilityVariantBySO(AbilitySO abilitySO)
    {
        Ability abilityVariant = GetAbilityByAbilitySO(abilitySO);

        if (abilityVariant == null)
        {
            if (debug) Debug.Log("Ability Variant is null. Selection will be ignored");
            return;
        }

        SelectAbiltiyVariant(abilityVariant);
    }

    private Ability GetAbilityByAbilitySO(AbilitySO abilitySO)
    {
        foreach(Ability abilityVariant in abilityVariants)
        {
            if(abilityVariant.AbilitySO == abilitySO) return abilityVariant;
        }

        if (debug) Debug.Log($"Could not find Ability Variant for AbilitySO with name : {abilitySO.abilityName}. Returning null.");
        return null;
    }
}
