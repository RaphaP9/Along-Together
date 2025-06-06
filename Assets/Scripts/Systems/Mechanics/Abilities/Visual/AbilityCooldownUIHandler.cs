using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityCooldownUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform cooldownPanelTransform;

    [Header("Components")]
    [SerializeField] private CooldownCounterUIHandler cooldownCounterUIHandler;

    [Header("Runtime Filled")]
    [SerializeField] private AbilityCooldownHandler abilityCooldownHandler;

    #region Flags
    private bool UIEnabled = false;
    #endregion

    private void Awake()
    {
        InitializeUI();
    }

    private void Update()
    {
        HandleCooldownUI();
    }

    private void InitializeUI()
    {
        DisableCooldownUI();
        UIEnabled = false;
    }

    private void HandleCooldownUI()
    {
        if (abilityCooldownHandler == null) return;

        //NOTE: CooldownCounterUIHandler does not update the cooldownText every frame (See CooldownCounterUIHandler class)
        cooldownCounterUIHandler.SetCooldownText(abilityCooldownHandler.CooldownTimer); 

        if(abilityCooldownHandler.CooldownTimer > 0 && !UIEnabled)
        {
            EnableCooldownUI();
            UIEnabled = true;
        }

        if (abilityCooldownHandler.CooldownTimer <= 0 && UIEnabled)
        {
            DisableCooldownUI();
            UIEnabled = false;
        }
    }

    #region PublicMethods
    public void AssignAbility(Ability ability)
    {
        switch (ability.AbilitySO.GetAbilityType())
        {
            case AbilityType.Passive:
                ClearAbilityCooldownHandler();
                break;
            case AbilityType.Active:
                SetAbilityCooldownHandler((ability as ActiveAbility).AbilityCooldownHandler);
                break;
            case AbilityType.ActivePassive:
                SetAbilityCooldownHandler((ability as ActivePassiveAbility).AbilityCooldownHandler);
                break;

        }
    }
    #endregion

    private void EnableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(true);
    private void DisableCooldownUI() => cooldownPanelTransform.gameObject.SetActive(false);

    #region Setters
    private void SetAbilityCooldownHandler(AbilityCooldownHandler abilityCooldownHandler) => this.abilityCooldownHandler = abilityCooldownHandler;
    private void ClearAbilityCooldownHandler() => abilityCooldownHandler = null;
    #endregion
}
