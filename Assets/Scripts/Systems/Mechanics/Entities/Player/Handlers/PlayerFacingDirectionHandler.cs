using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private PlayerAimDirectionerHandler aimDirectionerHandler;
    [SerializeField] private EntityHealth entityHealth;

    [Header("Settings")]
    [SerializeField] private FacingType facingType;
    [SerializeField] private Vector2Int startingFacingDirection;
    [SerializeField, Range(0.5f,10f)] private float minimumRigidbodyVelocity;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2Int currentFacingDirection;

    public Vector2Int CurrentFacingDirection => currentFacingDirection;

    private enum FacingType { Rigidbody, Aim };

    private void Start()
    {
        SetCurrentFacingDirection(startingFacingDirection);
    }

    private void Update()
    {
        HandleFacingDirection();
    }

    private void HandleFacingDirection()
    {
        if (!entityHealth.IsAlive()) return;

        switch (facingType)
        {
            case FacingType.Rigidbody:
            default:
                HandleFacingDirectionByRigidbody();
                break;
            case FacingType.Aim:
                HandleFacingDirectionByAim();
                break;

        }
    }

    private void HandleFacingDirectionByRigidbody()
    {
        if (_rigidbody2D.velocity.magnitude < minimumRigidbodyVelocity) return;

        Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(_rigidbody2D.velocity);

        if (currentFacingDirection != direction)
        {
            SetCurrentFacingDirection(direction);
        }
    }

    private void HandleFacingDirectionByAim()
    {
        Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(aimDirectionerHandler.AimDirection);

        if (currentFacingDirection != direction)
        {
            SetCurrentFacingDirection(direction);
        }
    }

    private void SetCurrentFacingDirection(Vector2Int facingDirection) => currentFacingDirection = facingDirection;

}
