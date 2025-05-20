using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttackInput : AttackInput
{
    private PlayerInputActions playerInputActions;

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Attack.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Attack.Disable();
    }

    public override bool CanProcessInput()
    {
        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;
        return true;
    }

    public override bool GetAttackDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attack.Attack.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAttackHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Attack.Attack.IsPressed();

        return input;
    }
}
