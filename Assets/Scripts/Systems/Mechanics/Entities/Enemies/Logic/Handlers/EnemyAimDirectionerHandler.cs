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
}
