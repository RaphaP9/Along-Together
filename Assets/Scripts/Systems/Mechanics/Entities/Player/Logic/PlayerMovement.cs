using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : EntityMovement
{
    [Header("Enabler")]
    [SerializeField] private bool movementEnabled;

    [Header("Components")]
    [SerializeField] private CharacterIdentifier characterIdentifier;
    [SerializeField] private PlayerHealth playerHealth;
    [Space]
    [SerializeField] private CheckWall checkWall;

    #region Properties
    public Vector2 DirectionInput => MovementInput.Instance.GetMovementInputNormalized();

    public float DesiredSpeed { get; private set; }
    public float SmoothCurrentSpeed { get; private set; }

    public Vector2 SmoothDirectionInput { get; private set; }
    public Vector2 FinalMoveValue { get; private set; }

    public Vector2 ScaledMovementVector { get; private set; }
    public bool MovementEnabled => movementEnabled;
    #endregion

    private void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #region Logic
    private void HandleMovement()
    {
        if (!movementEnabled) return;

        CalculateDesiredSpeed();
        SmoothSpeed();

        SmoothDirection();

        CalculateFinalMovement();
        ScaleFinalMovement();
    }

    private void CalculateDesiredSpeed()
    {
        DesiredSpeed = CanMove() ? GetMovementSpeedValue() : 0f;
    }

    private bool CanMove()
    {
        if (DirectionInput == Vector2.zero) return false;
        if (checkWall.HitWall) return false;
        if (!playerHealth.IsAlive()) return false;

        return true;
    }

    private void SmoothSpeed()
    {
        SmoothCurrentSpeed = Mathf.Lerp(SmoothCurrentSpeed, DesiredSpeed, Time.deltaTime * smoothVelocityFactor);
    }

    private void SmoothDirection() => SmoothDirectionInput = Vector2.Lerp(SmoothDirectionInput, DirectionInput, Time.deltaTime * smoothDirectionFactor);

    private void CalculateFinalMovement()
    {
        Vector2 finalInput = SmoothDirectionInput * SmoothCurrentSpeed;

        Vector2 roundedFinalInput;
        roundedFinalInput.x = Mathf.Abs(finalInput.x) < 0.01f ? 0f : finalInput.x;
        roundedFinalInput.y = Mathf.Abs(finalInput.y) < 0.01f ? 0f : finalInput.y;

        FinalMoveValue = roundedFinalInput;
    }

    private void ScaleFinalMovement()
    {
        ScaledMovementVector = MechanicsUtilities.ScaleVector2ToPerspective(FinalMoveValue);
    }

    private void ApplyMovement()
    {
        if (!CanApplyMovement()) return;

        _rigidbody2D.velocity = new Vector2(ScaledMovementVector.x, ScaledMovementVector.y);
    }
    #endregion

    private bool CanApplyMovement()
    {
        foreach (IDisplacementAbility displacementAbility in displacementAbilities)
        {
            if (displacementAbility.IsDisplacing()) return false;
        }

        return true;
    }

}
