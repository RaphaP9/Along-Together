using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastingTutorializedActionUI : TutorializedActionUI
{
    private bool eventConditionMet = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        Ability.OnAnyAbilityCast += Ability_OnAnyAbilityCast;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Ability.OnAnyAbilityCast -= Ability_OnAnyAbilityCast;
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.AbilityCasting;
    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;
        return eventConditionMet;
    }

    private void Ability_OnAnyAbilityCast(object sender, Ability.OnAbilityCastEventArgs e)
    {
        eventConditionMet = true;
    }
}
