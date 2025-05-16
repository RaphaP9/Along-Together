using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemyBehaviourHandler : EnemyBehaviourHandler
{
    [Header("Melee Enemy Components")]
    [SerializeField] private MeleeEnemyAttack meleeEnemyAttack;

    [Header("State - Runtime Filled")]
    [SerializeField] private MeleeEnemyState meleeEnemyState;

    private enum MeleeEnemyState {Spawning, FollowingPlayer, Attacking, Dead}
    private MeleeEnemySO MeleeEnemySO => enemyIdentifier.EnemySO as MeleeEnemySO;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;

        meleeEnemyAttack.OnEnemyAttackCompleted += MeleeEnemyAttack_OnMeleeEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;

        meleeEnemyAttack.OnEnemyAttackCompleted -= MeleeEnemyAttack_OnMeleeEnemyAttackCompleted;

        enemyHealth.OnEnemyDeath -= EnemyHealth_OnEnemyDeath;
    }

    private void Start()
    {
        SetState(MeleeEnemyState.Spawning);
    }

    private void Update()
    {
        HandleStates();
    }

    private void HandleStates()
    {
        switch (meleeEnemyState)
        {
            case MeleeEnemyState.Spawning:
                SpawningLogic();
                break;
            case MeleeEnemyState.FollowingPlayer:
                FollowingPlayerLogic();
                break;
            case MeleeEnemyState.Attacking:
                AttackingPlayerLogic();
                break;
            case MeleeEnemyState.Dead:
            default:
                DeadLogic();
                break;
        }
    }

    private void SpawningLogic() { }

    private void FollowingPlayerLogic()
    {
        enemyMovement.MoveTowardsPlayerDirection();

        if (OnAttackRange())
        {
            enemyMovement.StopOnCurrentPosition();
            meleeEnemyAttack.TriggerAttack();

            SetState(MeleeEnemyState.Attacking);
        }
    }

    private void AttackingPlayerLogic() { }

    private void DeadLogic() { }

    private void SetState(MeleeEnemyState state) => meleeEnemyState = state;
    private bool OnAttackRange() => playerRelativeHandler.DistanceToPlayer <= MeleeEnemySO.attackDistance;

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.Spawning);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.FollowingPlayer);
    }

    private void MeleeEnemyAttack_OnMeleeEnemyAttackCompleted(object sender, EnemyAttack.OnEntityAttackCompletedEventArgs e)
    {
        if (OnAttackRange())
        {
            meleeEnemyAttack.TriggerAttack();
        }
        else
        {
            SetState(MeleeEnemyState.FollowingPlayer);
        }
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EventArgs e)
    {
        meleeEnemyAttack.TriggerAttackStop();
        enemyMovement.StopOnCurrentPosition();

        SetState(MeleeEnemyState.Dead);
    }
    #endregion
}
