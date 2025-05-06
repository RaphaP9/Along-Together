using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTest : Ability, IActiveAbility, IDisplacementAbility, IDamageTakingInterruptionAbility
{
    [Header("Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private MovementDirectionHandler movementDirectionHandler;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Settings")]
    [SerializeField] private DirectionMode directionMode;
    [SerializeField] private bool interruptMovement;
    [SerializeField] private bool interruptDamageTaking;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 currentDashDirection;

    private DashTestSO DashTestSO => AbilitySO as DashTestSO;

    private enum DirectionMode { MousePosition, LastMovementDirection }

    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashTest;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashTestPre;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashTestStopped;

    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDashTest;
    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDashTestPre;
    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDashTestStopped;

    public class OnPlayerDashEventArgs : EventArgs
    {
        public Vector2 dashDirection;
        public bool interruptDamageTaking;
    }

    #region Interface Methods
    public bool AbilityInput() => GetAssociatedDownInput();
    public bool CanInterruptMovement() => interruptMovement;
    public bool IsDisplacing()
    {
        return false;
    }

    public bool CanInterruptDamageTaking() => interruptDamageTaking;

    public bool IsInterruptingDamageTaking()
    {
        return false;
    }

    #endregion
}
