using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastingTutorializedActionUI : TutorializedActionUI
{
    private bool eventConditionMet = false;
    private AbilitySlot lastAbilitySlotUpgraded;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ability.OnAnyAbilityCast += Ability_OnAnyAbilityCast;
        AbilityLevelHandler.OnAnyAbilityLevelSet += AbilityLevelHandler_OnAnyAbilityLevelSet;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Ability.OnAnyAbilityCast -= Ability_OnAnyAbilityCast;
        AbilityLevelHandler.OnAnyAbilityLevelSet -= AbilityLevelHandler_OnAnyAbilityLevelSet;
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.AbilityCasting;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        return eventConditionMet;
    }

    #region Subscriptions
    private void Ability_OnAnyAbilityCast(object sender, Ability.OnAbilityCastEventArgs e)
    {
        eventConditionMet = true;
    }

    private void AbilityLevelHandler_OnAnyAbilityLevelSet(object sender, AbilityLevelHandler.OnAbilityLevelEventArgs e)
    {
        lastAbilitySlotUpgraded = e.ability.AbilitySlot;
    }
    #endregion
}
