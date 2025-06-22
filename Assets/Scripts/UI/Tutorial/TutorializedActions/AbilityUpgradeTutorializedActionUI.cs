using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeTutorializedActionUI : TutorializedActionUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose += AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately += AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose -= AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately -= AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.AbilityUpgrade;
    protected override bool CheckCondition() => false;

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeClose(object sender, EventArgs e)
    {
        if (!isActive) return;
        if (tutorialConditionMet) return;

        tutorialConditionMet = true;
        CloseTutorializedAction();
    }

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately(object sender, EventArgs e)
    {
        if (!isActive) return;
        if (tutorialConditionMet) return;

        tutorialConditionMet = true;
        CloseTutorializedAction();
    }
}
