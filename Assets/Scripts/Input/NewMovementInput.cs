using System;
using UnityEngine;

public class NewMovementInput : MovementInput
{
    private PlayerInputActions playerInputActions;

    private Vector2 LastNonZeroMovementInput = new Vector2(1f, 0f); //Default Value Assigned

    protected override void Awake()
    {
        base.Awake();
        InitializePlayerInputActions();
    }

    private void InitializePlayerInputActions()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Movement.Disable();
    }

    private void Update()
    {
        CalculateLastNonZeroInput();
    }

    private void CalculateLastNonZeroInput()
    {
        if (GetMovementInputNormalized() == Vector2.zero) return;
        
        LastNonZeroMovementInput = GetMovementInputNormalized();
        
    }

    public override bool CanProcessInput()
    {
        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;
        return true;
    }

    public override Vector2 GetMovementInputNormalized()
    {
        if (!CanProcessInput()) return Vector2.zero;

        Vector2 input = playerInputActions.Movement.Move.ReadValue<Vector2>();

        return input;
    }

    public override Vector2 GetLastNonZeroMovementInputNormalized() => LastNonZeroMovementInput;
}
