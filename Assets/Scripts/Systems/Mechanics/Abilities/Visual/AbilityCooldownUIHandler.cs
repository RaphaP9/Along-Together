using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityCooldownUIHandler : MonoBehaviour
{
    [Header("UIComponents")]
    [SerializeField] private Transform cooldownPanelTransform;
    [SerializeField] private TextMeshProUGUI cooldownText;

    [Header("Runtime Filled")]
    [SerializeField] private Ability ability;
    [SerializeField] private AbilityCooldownHandler abilityCooldownHandler;

    private void Update()
    {
        HandleCooldownText();
    }

    private void HandleCooldownText()
    {

    }

    #region PublicMethods
    public void AssignAbility(Ability ability)
    {
        SetAbility(ability);

        switch (ability.AbilitySO.GetAbilityType())
        {
            case AbilityType.Passive:
                AssignPassiveAbility(ability as PassiveAbility);
                break;
            case AbilityType.Active:
                AssignActiveAbilityAbility(ability as ActiveAbility);
                break;
            case AbilityType.ActivePassive:
                AssignActivePassiveAbilityAbility(ability as ActivePassiveAbility);
                break;

        }
    }
    #endregion

    #region Logics
    private void AssignPassiveAbility(PassiveAbility passiveAbility)
    {
        ClearAbilityCooldownHandler();
    }

    private void AssignActiveAbilityAbility(ActiveAbility activeAbility)
    {
        SetAbilityCooldownHandler(activeAbility.AbilityCooldownHandler);
    }

    private void AssignActivePassiveAbilityAbility(ActivePassiveAbility activePassiveAbility)
    {
        SetAbilityCooldownHandler(activePassiveAbility.AbilityCooldownHandler);
    }
    #endregion

    private void EnableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(true);
    private void DisableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(false);

    #region Setters
    private void SetAbilityCooldownHandler(AbilityCooldownHandler abilityCooldownHandler) => this.abilityCooldownHandler = abilityCooldownHandler;
    private void ClearAbilityCooldownHandler() => abilityCooldownHandler = null;

    private void SetAbility(Ability ability) => this.ability = ability;
    private void ClearAbility() => ability = null;
    #endregion
}
