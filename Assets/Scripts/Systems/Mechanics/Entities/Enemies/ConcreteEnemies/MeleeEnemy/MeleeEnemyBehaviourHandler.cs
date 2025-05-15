using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemyBehaviourHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemySpawnHandler enemySpawnHandler;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EnemyCleanup enemyCleanup;

    [Header("State - Runtime Filled")]
    [SerializeField] private MeleeEnemyState meleeEnemyState;

    private enum MeleeEnemyState {Spawning, FollowingPlayer, AttackingPlayer, Dead}
    private MeleeEnemySO MeleeEnemySO => enemyIdentifier.EnemySO as MeleeEnemySO;

    private void OnEnable()
    {
        enemySpawnHandler.OnEnemySpawnStart += EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete += EnemySpawnHandler_OnEnemySpawnComplete;
        enemyHealth.OnEnemyDeath += EnemyHealth_OnEnemyDeath;
    }

    private void OnDisable()
    {
        enemySpawnHandler.OnEnemySpawnStart -= EnemySpawnHandler_OnEnemySpawnStart;
        enemySpawnHandler.OnEnemySpawnComplete -= EnemySpawnHandler_OnEnemySpawnComplete;
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
            case MeleeEnemyState.AttackingPlayer:
                AttackingPlayerLogic();
                break;
            case MeleeEnemyState.Dead:
            default:
                DeadLogic();
                break;
        }
    }

    private void SpawningLogic()
    {
        //
    }

    private void FollowingPlayerLogic()
    {

    }

    private void AttackingPlayerLogic()
    {

    }

    private void DeadLogic()
    {
        //
    }

    private void SetState(MeleeEnemyState state) => meleeEnemyState = state;

    #region Susbcriptions

    private void EnemySpawnHandler_OnEnemySpawnStart(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.Spawning);
    }

    private void EnemySpawnHandler_OnEnemySpawnComplete(object sender, EnemySpawnHandler.OnEnemySpawnEventArgs e)
    {
        SetState(MeleeEnemyState.FollowingPlayer);
    }

    private void EnemyHealth_OnEnemyDeath(object sender, EventArgs e)
    {
        SetState(MeleeEnemyState.Dead);
    }
    #endregion
}
