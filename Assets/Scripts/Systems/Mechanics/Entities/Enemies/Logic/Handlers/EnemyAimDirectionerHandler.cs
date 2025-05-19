using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimDirectionerHandler : EntityAimDirectionerHandler
{
    [Header("Enemy Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;

    protected override Vector2 CalculateAimDirection() => playerRelativeHandler.DirectionToPlayer;
    protected override float CalculateAimAngle() => playerRelativeHandler.AngleToPlayer;

    protected override Vector2 CalculateRefferencedAimDirection()
    {
        Vector2 rawRefferencedDirection = playerRelativeHandler.PlayerPosition - GeneralUtilities.TransformPositionVector2(refferenceAimPoint);
        return rawRefferencedDirection.normalized;
    }

    protected override float CalculateRefferencedAimAngle() => GeneralUtilities.GetVector2AngleDegrees(CalculateRefferencedAimDirection());

    protected override bool CanAim()
    {
        if (!base.CanAim()) return false;
        if(enemySpawnHandler.IsSpawning) return false;

        return true;
    }
}
