using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastingTutorializedActionUI : TutorializedActionUI
{
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
    protected override bool CheckCondition() => false;

    private void Ability_OnAnyAbilityCast(object sender, Ability.OnAbilityCastEventArgs e)
    {
        if (!isActive) return;
        if (tutorialConditionMet) return;

        tutorialConditionMet = true;
        CloseTutorializedAction();
    }
}
