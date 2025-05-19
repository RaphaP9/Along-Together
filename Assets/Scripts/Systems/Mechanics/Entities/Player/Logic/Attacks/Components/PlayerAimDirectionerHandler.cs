using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimDirectionerHandler : EntityAimDirectionerHandler
{
    [Header("Player Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;

    protected override Vector2 CalculateAimDirection() => mouseDirectionHandler.NormalizedMouseDirection;
    protected override float CalculateAimAngle() => GeneralUtilities.GetVector2AngleDegrees(CalculateAimDirection());

    protected override Vector2 CalculateRefferencedAimDirection()
    {
        Vector2 rawRefferencedDirection = mouseDirectionHandler.Input - GeneralUtilities.TransformPositionVector2(refferenceAimPoint);
        return rawRefferencedDirection.normalized;
    }

    protected override float CalculateRefferencedAimAngle() => GeneralUtilities.GetVector2AngleDegrees(CalculateRefferencedAimDirection());
}
