using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedInputSystemManager : MonoBehaviour
{
    public static CentralizedInputSystemManager Instance { get; private set; }

    public PlayerInputActions PlayerInputActions { get; private set; }

    public static EventHandler<OnPlayerInputActionsEventArgs> OnPlayerInputActionsInitialized;   

    public class OnPlayerInputActionsEventArgs : EventArgs
    {
        public PlayerInputActions playerInputActions;
    }

    private void Awake()
    {
        SetSingleton();
        InitializePlayerInputActions();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayerInputActions()
    {
        PlayerInputActions = new PlayerInputActions();
        OnPlayerInputActionsInitialized?.Invoke(this , new OnPlayerInputActionsEventArgs { playerInputActions = PlayerInputActions });
    }

    public void DisableAllActionMaps()
    {
        PlayerInputActions.UI.Disable();
        PlayerInputActions.Movement.Disable();
        PlayerInputActions.Attack.Disable();
        PlayerInputActions.Abilities.Disable();
        PlayerInputActions.Conversations.Disable();
    }

    public void EnableAllActionMaps()
    {
        PlayerInputActions.UI.Enable();
        PlayerInputActions.Movement.Enable();
        PlayerInputActions.Attack.Enable();
        PlayerInputActions.Abilities.Enable();
        PlayerInputActions.Conversations.Enable();
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return PlayerInputActions.Movement.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return PlayerInputActions.Movement.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return PlayerInputActions.Movement.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return PlayerInputActions.Movement.Move.bindings[4].ToDisplayString();
            case Binding.Attack:
                return PlayerInputActions.Attack.Attack.bindings[0].ToDisplayString();
            case Binding.Ability1:
                return PlayerInputActions.Abilities.AbilityA.bindings[0].ToDisplayString();
            case Binding.Ability2:
                return PlayerInputActions.Abilities.AbilityB.bindings[0].ToDisplayString();
            case Binding.Ability3:
                return PlayerInputActions.Abilities.AbilityC.bindings[0].ToDisplayString();
            case Binding.SkipDialogue:
                return PlayerInputActions.Conversations.Skip.bindings[0].ToDisplayString();
            case Binding.Stats:
                return PlayerInputActions.UI.Stats.bindings[0].ToDisplayString();
            case Binding.DevMenu:
                return PlayerInputActions.UI.DevMenu.bindings[0].ToDisplayString();
            case Binding.Pause:
                return PlayerInputActions.UI.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding)
    {
        DisableAllActionMaps();

        
    }
}
