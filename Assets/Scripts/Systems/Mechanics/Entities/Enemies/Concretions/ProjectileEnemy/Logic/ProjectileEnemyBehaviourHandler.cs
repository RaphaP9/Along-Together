using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyBehaviourHandler : EnemyBehaviourHandler
{
    [Header("Projectile Enemy Components")]
    [SerializeField] private ProjectileEnemyAttack projectileEnemyAttack;

    [Header("State - Runtime Filled")]
    [SerializeField] private ProjectileEnemyState projectileEnemyState;

    private enum ProjectileEnemyState { Spawning, FollowingPlayer, Attacking, Dead }
    private ProjectileEnemySO ProjectileEnemySO => enemyIdentifier.EnemySO as ProjectileEnemySO;
}
