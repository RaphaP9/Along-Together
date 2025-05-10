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
    [SerializeField] private Ability activeAbilityVariant;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public AbilitySlot AbilitySlot => abilitySlot;
    public Ability ActiveAbilityVariant => activeAbilityVariant;

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

    private void Update()
    {
        HandleAbilityCasting();
    }

    //All abilities can be cast. If only passive, cast is always denied
    protected virtual void HandleAbilityCasting()
    {
        if (!GetAssociatedDownInput()) return;

        activeAbilityVariant.TryCastAbility();
    }

    #region Ability Variants Selection
    private void InitializeAbilityVariant(Ability abilityVariant)
    {
        Ability previousAbilityVariant = null;
        activeAbilityVariant = abilityVariant;

        OnAbilityVariantInitialized?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlot = abilitySlot, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = activeAbilityVariant });
    }

    private void SelectAbilityVariant(Ability abilityVariant)
    {
        Ability previousAbilityVariant = activeAbilityVariant;
        activeAbilityVariant = abilityVariant;

        OnAbilityVariantSelected?.Invoke(this, new OnAbilityVariantSelectionEventArgs { abilitySlot = abilitySlot, previousAbilityVariant = previousAbilityVariant, newAbilityVariant = activeAbilityVariant });
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
    #endregion



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
            if (debug) Debug.Log($"Ability with name: {setterPrimitiveAbilitySlotGroup.abilitySO.abilityName} is not found in Slot: {abilitySlot}. Setting will be ignored and starting variant will be as set in inspector.");
            return;
        }

        startingAbilityVariant = ability;
    }

    public PrimitiveAbilitySlotGroup GetPrimitiveAbilitySlotGroup()
    {
        PrimitiveAbilitySlotGroup primitiveAbilitySlotGroup = new PrimitiveAbilitySlotGroup { abilitySlot = abilitySlot, abilitySO = activeAbilityVariant.AbilitySO };
        return primitiveAbilitySlotGroup;
    }
    #endregion

    #region Input Association
    public bool GetAssociatedDownInput()
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

    public bool GetAsociatedHoldInput()
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
}
