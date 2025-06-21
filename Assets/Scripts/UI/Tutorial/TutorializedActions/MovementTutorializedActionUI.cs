using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTutorializedActionUI : TutorializedActionUI
{
    [Header("Specific Settings")]
    [SerializeField, Range(10f, 50f)] private float distanceCoveredToMetTutorializationCondition;

    protected override void OpenTutorializedAction()
    {
        PlayerDistanceCoveredManager.Instance.ResetDistanceCovered();
        base.OpenTutorializedAction();
    }

    public override TutorializedAction GetTutorializedAction() => TutorializedAction.Movement;

    protected override bool CheckCondition()
    {
        if (PlayerDistanceCoveredManager.Instance.PlayerDistanceCovered >= distanceCoveredToMetTutorializationCondition) return true;
        return false;
    }
}
