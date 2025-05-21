using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerAbilitiesCastingHandler playerAbilitiesCastingHandler;
    [Space]
    [SerializeField] private PlayerFacingDirectionHandler playerFacingDirectionHandler;
    [SerializeField] private PlayerWeaponAimHandler playerWeaponAimHandler;

    [Header("Settings")]
    [SerializeField] private PlayerState startingState;

    [Header("Runtime Filled")]
    [SerializeField] private PlayerState playerState;

    private enum PlayerState {Spawning, Combat, Rest, Dead}

    private void Start()
    {
        SetPlayerState(startingState);
    }

    private void Update()
    {
        HandleStateUpdate();
    }

    private void FixedUpdate()
    {
        HandleStateFixedUpdate();
    }

    private void LateUpdate()
    {
        HandleStateLateUpdate();
    }

    private void HandleStateUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Spawning:
                SpawningLogicUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicUpdate();
                break;
            case PlayerState.Rest:
                RestLogicUpdate();
                break;
            case PlayerState.Dead:
                DeadLogicUpdate();
                break;
        }
    }

    private void HandleStateFixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Spawning:
                SpawningLogicFixedUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicFixedUpdate();
                break;
            case PlayerState.Rest:
                RestLogicFixedUpdate();
                break;
            case PlayerState.Dead:
                DeadLogicFixedUpdate();
                break;
        }
    }

    private void HandleStateLateUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Spawning:
                SpawningLogicLateUpdate();
                break;
            case PlayerState.Combat:
            default:
                CombatLogicLateUpdate();
                break;
            case PlayerState.Rest:
                RestLogicLateUpdate();
                break;
            case PlayerState.Dead:
                DeadLogicLateUpdate();
                break;
        }
    }

    #region Spawning Logics
    private void SpawningLogicUpdate()
    {

    }

    private void SpawningLogicFixedUpdate()
    {

    }

    private void SpawningLogicLateUpdate()
    {

    }

    #endregion

    #region Combat Logics
    private void CombatLogicUpdate()
    {
        playerMovement.HandleMovement();
        playerAttack.HandleAttack();
        playerAbilitiesCastingHandler.HandleAbilitiesCasting();
        playerFacingDirectionHandler.HandleFacing();
    }

    private void CombatLogicFixedUpdate()
    {
        playerMovement.ApplyMovement();
    }

    private void CombatLogicLateUpdate()
    {
        playerWeaponAimHandler.HandlePivotRotation();
    }
    #endregion

    #region Rest Logics
    private void RestLogicUpdate()
    {
        playerMovement.HandleMovement();
        playerFacingDirectionHandler.HandleFacing();
    }

    private void RestLogicFixedUpdate()
    {
        playerMovement.ApplyMovement();
    }

    private void RestLogicLateUpdate()
    {
        playerWeaponAimHandler.HandlePivotRotation();
    }
    #endregion

    #region Dead Logics
    private void DeadLogicUpdate()
    {

    }

    private void DeadLogicFixedUpdate()
    {

    }

    private void DeadLogicLateUpdate()
    {

    }
    #endregion

    private void SetPlayerState(PlayerState state) => playerState = state;
}
