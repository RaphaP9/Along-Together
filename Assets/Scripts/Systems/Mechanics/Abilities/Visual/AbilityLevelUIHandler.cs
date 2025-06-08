using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLevelUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform notLearnedPanelTransform;

    [Header("Runtime Filled")]
    [SerializeField] private Ability ability;

    private bool firstUpdateLogicPerformed = false;


    private void OnEnable()
    {
        AbilityLevelHandler.OnAnyAbilityLevelInitialized += AbilityLevelHandler_OnAnyAbilityLevelInitialized;
        AbilityLevelHandler.OnAnyAbilityLevelSet += AbilityLevelHandler_OnAnyAbilityLevelSet;
    }

    private void OnDisable()
    {
        AbilityLevelHandler.OnAnyAbilityLevelInitialized -= AbilityLevelHandler_OnAnyAbilityLevelInitialized;
        AbilityLevelHandler.OnAnyAbilityLevelSet -= AbilityLevelHandler_OnAnyAbilityLevelSet;
    }


    private void Update()
    {
        HandleFirstUpdateLogic();
    }

    private void HandleFirstUpdateLogic() //Performed in first update to make sure AbilityLevelHandler has already initialized the AbilityLevel (Initializes on Start())
    {
        if (firstUpdateLogicPerformed) return;

        firstUpdateLogicPerformed = true;

        if (ability == null) return;
        HandleUI(ability.AbilityLevel);
    }

    private void HandleUI(AbilityLevel abilityLevel)
    {
        if (abilityLevel == AbilityLevel.NotLearned)
        {
            EnableNotLearnedUI();
        }
        else
        {
            DisableLearnedUI();
        }
    }

    #region Public Methods
    public void AssignAbility(Ability ability)
    {
        SetAbility(ability);

        if (!firstUpdateLogicPerformed) return;

        HandleUI(ability.AbilityLevel);
    }
    #endregion

    private void EnableNotLearnedUI() => notLearnedPanelTransform.gameObject.SetActive(true);
    private void DisableLearnedUI() => notLearnedPanelTransform.gameObject.SetActive(false);

    #region Setters
    private void SetAbility(Ability ability) => this.ability = ability;
    private void ClearAbility() => ability = null;
    #endregion

    #region Subscriptions
    private void AbilityLevelHandler_OnAnyAbilityLevelInitialized(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        if (ability?.AbilitySO != e.abilitySO) return;

        HandleUI(e.abilityLevel);
    }

    private void AbilityLevelHandler_OnAnyAbilityLevelSet(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        if (ability?.AbilitySO != e.abilitySO) return;

        HandleUI(e.abilityLevel);
    }
    #endregion
}
