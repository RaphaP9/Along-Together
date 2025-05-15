using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAimDirectionerHandler : EntityAimDirectionerHandler
{
    [Header("Enemy Components")]
    [SerializeField] private PlayerRelativeHandler playerRelativeHandler;

    protected override Vector2 CalculateAimDirection() => playerRelativeHandler.DirectionToPlayer;
    protected override float CalculateAimAngle() => playerRelativeHandler.AngleToPlayer;
}
