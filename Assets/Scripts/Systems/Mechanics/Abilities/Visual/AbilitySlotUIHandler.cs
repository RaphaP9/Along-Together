using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySlotHandler abilitySlotHandler;
    [SerializeField] private AbilityCooldownUIHandler abilityCooldownUIHandler;

    [Header("UI Components")]
    [SerializeField] private Image abilityImage; 

    private void OnEnable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized += AbilitySlotHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected += AbilitySlotHandler_OnAbilityVariantSelected;
    }

    private void OnDisable()
    {
        abilitySlotHandler.OnAbilityVariantInitialized -= AbilitySlotHandler_OnAbilityVariantInitialized;
        abilitySlotHandler.OnAbilityVariantSelected -= AbilitySlotHandler_OnAbilityVariantSelected;
    }

    private void SetAbilityImage(Sprite sprite) => abilityImage.sprite = sprite;

    private void AbilitySlotHandler_OnAbilityVariantInitialized(object sender, AbilitySlotHandler.OnAbilityVariantInitializationEventArgs e)
    {
        SetAbilityImage(e.abilityVariant.AbilitySO.sprite);
        abilityCooldownUIHandler.AssignAbility(e.abilityVariant);
    }
    private void AbilitySlotHandler_OnAbilityVariantSelected(object sender, AbilitySlotHandler.OnAbilityVariantSelectionEventArgs e)
    {
        SetAbilityImage(e.newAbilityVariant.AbilitySO.sprite);
        abilityCooldownUIHandler.AssignAbility(e.newAbilityVariant);
    }
}
