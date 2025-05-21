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

    [Header("States")]
    [SerializeField] private PlayerState playerState;

    private enum PlayerState { Spawning, Combat, Rest, Dead}

    private void Start()
    {
        SetPlayerState(PlayerState.Combat);
    }

    private void Update()
    {
        HandleStateUpdate();
    }

    private void FixedUpdate()
    {
        HandleStateFixedUpdate();
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

    #region Spawning Logics
    private void SpawningLogicUpdate()
    {

    }

    private void SpawningLogicFixedUpdate()
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
        playerFacingDirectionHandler.HandleFacing();
    }
    #endregion

    #region Rest Logics
    private void RestLogicUpdate()
    {
        playerMovement.HandleMovement();
    }

    private void RestLogicFixedUpdate()
    {
        playerMovement.ApplyMovement();
    }
    #endregion

    #region Rest Logics
    private void DeadLogicUpdate()
    {

    }

    private void DeadLogicFixedUpdate()
    {

    }
    #endregion

    private void SetPlayerState(PlayerState state) => playerState = state;
}
