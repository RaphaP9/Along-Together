using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CentralizedInputSystemManager : MonoBehaviour
{
    public static CentralizedInputSystemManager Instance { get; private set; }

    public PlayerInputActions PlayerInputActions { get; private set; }

    public static EventHandler<OnPlayerInputActionsEventArgs> OnPlayerInputActionsInitialized;

    public static event EventHandler<OnRebindingEventArgs> OnRebindingStarted;
    public static event EventHandler<OnRebindingEventArgs> OnRebindingCompleted;

    public class OnPlayerInputActionsEventArgs : EventArgs
    {
        public PlayerInputActions playerInputActions;
    }

    public class OnRebindingEventArgs : EventArgs
    {
        public Binding binding;
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

    #region Getters
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return PlayerInputActions.Movement.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return PlayerInputActions.Movement.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return PlayerInputActions.Movement.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
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

    private InputAction GetBindingInputAction(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
            case Binding.MoveDown:
            case Binding.MoveLeft:
            case Binding.MoveRight:
                return PlayerInputActions.Movement.Move;
            case Binding.Attack:
                return PlayerInputActions.Attack.Attack;
            case Binding.Ability1:
                return PlayerInputActions.Abilities.AbilityA;
            case Binding.Ability2:
                return PlayerInputActions.Abilities.AbilityB;
            case Binding.Ability3:
                return PlayerInputActions.Abilities.AbilityC;
            case Binding.SkipDialogue:
                return PlayerInputActions.Conversations.Skip;
            case Binding.Stats:
                return PlayerInputActions.UI.Stats;
            case Binding.DevMenu:
                return PlayerInputActions.UI.DevMenu;
            case Binding.Pause:
                return PlayerInputActions.UI.Pause;
        }
    }

    private int GetBindingIndex(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return 1;
            case Binding.MoveDown:
                return 2;
            case Binding.MoveLeft:
                return 3;
            case Binding.MoveRight:
                return 4;
            case Binding.Attack:
            case Binding.Ability1:
            case Binding.Ability2:
            case Binding.Ability3:
            case Binding.SkipDialogue:
            case Binding.Stats:
            case Binding.DevMenu:
            case Binding.Pause:
                return 0;
        }
    }
    #endregion

    public void RebindBinding(Binding binding)
    {
        OnRebindingStarted?.Invoke(this, new OnRebindingEventArgs { binding = binding });
        DisableAllActionMaps();

        InputAction inputAction = GetBindingInputAction(binding);
        int bindingIndex = GetBindingIndex(binding);

        inputAction.PerformInteractiveRebinding(bindingIndex).
            OnComplete(callback =>
            {
                callback.Dispose();
                EnableAllActionMaps();
                OnRebindingCompleted?.Invoke(this, new OnRebindingEventArgs { binding = binding });
            });
    }
}
