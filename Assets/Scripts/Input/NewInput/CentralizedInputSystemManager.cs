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
}
