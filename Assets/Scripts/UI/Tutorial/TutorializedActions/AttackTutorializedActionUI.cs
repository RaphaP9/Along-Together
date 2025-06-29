using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTutorializedActionUI : TutorializedActionUI
{
    [Header("Specific Settings")]
    [SerializeField, Range(5, 20f)] private int attacksPerformedToMetTutorializationCondition;

    protected override void OpenTutorializedAction()
    {
        PlayerAttackCounterManager.Instance.ResetAttacksPerformed();
        base.OpenTutorializedAction();
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.Attack;

    protected override bool CheckCondition()
    {
        if (!isDetectingCondition) return false;

        if (PlayerAttackCounterManager.Instance.AttacksPerformed >= attacksPerformedToMetTutorializationCondition) return true;
        return false;
    }
}
