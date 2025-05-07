using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DashTest : Ability, IActiveAbility, IDisplacementAbility, IDamageTakingInterruptionAbility
{
    [Header("Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private MovementDirectionHandler movementDirectionHandler;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [Space]
    [SerializeField] private AbilityCooldownHandler abilityCooldownHandler;

    [Header("Settings")]
    [SerializeField] private DirectionMode directionMode;
    [SerializeField] private bool interruptMovement;
    [SerializeField] private bool interruptDamageTaking;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 currentDashDirection;

    private DashTestSO DashTestSO => AbilitySO as DashTestSO;
    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;

    private enum DirectionMode { MousePosition, LastMovementDirection }

    private bool isDashing = false;
    private float dashPerformTimer = 0f;
    private bool shouldDash = false;

    #region Events

    public static event EventHandler<OnAbilityCastEventArgs> OnDashCast;
    public static event EventHandler<OnAbilityCastEventArgs> OnDashCastDenied;

    public event EventHandler<OnAbilityCastEventArgs> OnThisDashCast;
    public event EventHandler<OnAbilityCastEventArgs> OnThisDashCastDenied;

    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDash;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashPre;
    public static event EventHandler<OnPlayerDashEventArgs> OnPlayerDashStopped;

    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDash;
    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDashPre;
    public event EventHandler<OnPlayerDashEventArgs> OnThisPlayerDashStopped;

    #endregion

    public class OnPlayerDashEventArgs : EventArgs
    {
        public Vector2 dashDirection;
    }

    #region Interface Methods
    public bool AbilityInput() => GetAssociatedDownInput();
    public bool CanInterruptMovement() => interruptMovement;
    public bool IsDisplacing() => isDashing;

    public bool CanInterruptDamageTaking() => interruptDamageTaking;
    public bool IsInterruptingDamageTaking() => isDashing;
    #endregion


    #region Logic

    protected override void HandleUpdateLogic()
    {
        if (dashPerformTimer > 0) dashPerformTimer -= Time.deltaTime;
        else if (isDashing) StopDash();
    }

    protected override void HandleFixedUpdateLogic()
    {
        if (!shouldDash) return;

        Dash();
        SetDashPerformTimer(DashTestSO.dashTime);
        shouldDash = false;
    }

    #endregion


    #region Abstract Methods
    protected override void OnAbilityCastMethod()
    {
        base.OnAbilityCastMethod();

        OnDashCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnThisDashCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });

        shouldDash = true;
        abilityCooldownHandler.SetCooldownTimer(DashTestSO.baseCooldown); //Replace with some resolver method (AbilityCooldownReductionResolver)
    }

    protected override void OnAbilityCastDeniedMethod()
    {
        base.OnAbilityCastDeniedMethod();

        OnDashCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnThisDashCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
    }

    public override bool CanCastAbility()
    {
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }
    #endregion

    #region AbilitySpecifics
    public void Dash()
    {
        currentDashDirection = DefineDashDirection();

        float dashForce = DashTestSO.dashDistance / DashTestSO.dashTime;
        Vector2 dashVector = currentDashDirection * dashForce;

        Vector2 scaledDashVector = MechanicsUtilities.ScaleVector2ToPerspective(dashVector);

        OnPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection});
        OnThisPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection});

        _rigidbody2D.velocity = scaledDashVector;
        isDashing = true;

        OnPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnThisPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
    }

    private void StopDash()
    {
        if (!isDashing) return;

        _rigidbody2D.velocity = Vector2.zero;
        isDashing = false;

        OnPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnThisPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });

        currentDashDirection = Vector2.zero;
    }

    private Vector2 DefineDashDirection()
    {
        switch (directionMode)
        {
            case DirectionMode.MousePosition:
                return mouseDirectionHandler.NormalizedMouseDirection;
            case DirectionMode.LastMovementDirection:
            default:
                return movementDirectionHandler.LastMovementDirection;
        }
    }

    private void HandleDashResistance()
    {
        if (!isDashing) return;

        Vector2 dashResistanceForce = -currentDashDirection * DashTestSO.dashResistance;
        _rigidbody2D.AddForce(dashResistanceForce, ForceMode2D.Force);
    }

    private void SetDashPerformTimer(float time) => dashPerformTimer = time;

    #endregion
}
