using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DashTest : Ability, IActiveAbility, IDisplacementAbility, IDamageTakingInterruptionAbility
{
    [Header("Specific Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private MovementDirectionHandler movementDirectionHandler;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [Space]
    [SerializeField] private AbilityCooldownHandler abilityCooldownHandler;

    [Header("Specific Settings")]
    [SerializeField] private DirectionMode directionMode;
    [SerializeField] private bool interruptMovement;
    [SerializeField] private bool interruptDamageTaking;

    [Header("Specific Runtime Filled")]
    [SerializeField] private Vector2 currentDashDirection;

    private DashTestSO DashTestSO => AbilitySO as DashTestSO;
    public AbilityCooldownHandler AbilityCooldownHandler => abilityCooldownHandler;

    private enum DirectionMode { MousePosition, LastMovementDirection }

    private bool isDashing = false;
    private float dashPerformTimer = 0f;
    private bool shouldDash = false;

    #region Events

    public static event EventHandler<OnAbilityCastEventArgs> OnAnyDashCast;
    public static event EventHandler<OnAbilityCastEventArgs> OnAnyDashCastDenied;

    public event EventHandler<OnAbilityCastEventArgs> OnDashCast;
    public event EventHandler<OnAbilityCastEventArgs> OnDashCastDenied;

    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDash;
    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDashPre;
    public static event EventHandler<OnPlayerDashEventArgs> OnAnyPlayerDashStopped;

    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDash;
    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDashPre;
    public event EventHandler<OnPlayerDashEventArgs> OnPlayerDashStopped;

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
        if (!isActiveVariant) return;
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

        OnAnyDashCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnDashCast?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });

        shouldDash = true;
        abilityCooldownHandler.SetCooldownTimer(DashTestSO.baseCooldown); //Replace with some resolver method (AbilityCooldownReductionResolver)
    }

    protected override void OnAbilityCastDeniedMethod()
    {
        base.OnAbilityCastDeniedMethod();

        OnAnyDashCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
        OnDashCastDenied?.Invoke(this, new OnAbilityCastEventArgs { abilitySO = abilitySO });
    }

    public override bool CanCastAbility()
    {
        if (!playerHealth.IsAlive()) return false;
        if (abilityLevel == AbilityLevel.NotLearned) return false;
        if (abilityCooldownHandler.IsOnCooldown()) return false;

        return true;
    }

    protected override void ActivateAbilityVariant()
    {
        base.ActivateAbilityVariant();
    }

    protected override void DisableAbilityVariant()
    {
        base.ActivateAbilityVariant();
        StopDash();
    }
    #endregion

    #region AbilitySpecifics
    public void Dash()
    {
        currentDashDirection = DefineDashDirection();

        float dashForce = DashTestSO.dashDistance / DashTestSO.dashTime;
        Vector2 dashVector = currentDashDirection * dashForce;

        Vector2 scaledDashVector = MechanicsUtilities.ScaleVector2ToPerspective(dashVector);

        OnAnyPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection});
        OnPlayerDashPre?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection});

        _rigidbody2D.velocity = scaledDashVector;
        isDashing = true;

        OnAnyPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnPlayerDash?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
    }

    private void StopDash()
    {
        if (!isDashing) return;

        _rigidbody2D.velocity = Vector2.zero;
        isDashing = false;

        OnAnyPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });
        OnPlayerDashStopped?.Invoke(this, new OnPlayerDashEventArgs { dashDirection = currentDashDirection });

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
