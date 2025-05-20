using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbilitiesInput : AbilitiesInput
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
        playerInputActions.Abilities.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Abilities.Disable();
    }

    public override bool CanProcessInput()
    {
        //if (GameManager.Instance.GameState != GameManager.State.OnWave) return false;
        if (PauseManager.Instance.GamePaused) return false;
        return true;
    }

    public override bool GetAbilityADown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityA.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityAHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityA.IsPressed();

        return input;
    }

    public override bool GetAbilityBDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityB.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityBHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityB.IsPressed();

        return input;
    }

    public override bool GetAbilityCDown()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityC.WasPerformedThisFrame();

        return input;
    }

    public override bool GetAbilityCHold()
    {
        if (!CanProcessInput()) return false;

        bool input = playerInputActions.Abilities.AbilityC.IsPressed();

        return input;
    }
}
