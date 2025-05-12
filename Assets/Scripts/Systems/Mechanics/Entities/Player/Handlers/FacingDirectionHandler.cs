using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingDirectionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private EntityHealth entityHealth;

    [Header("Settings")]
    [SerializeField] private Vector2Int startingFacingDirection;
    [SerializeField, Range(0.5f,10f)] private float minimumVelocity;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2Int currentFacingDirection;

    public Vector2Int CurrentFacingDirection => currentFacingDirection;


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

        if (_rigidbody2D.velocity.magnitude < minimumVelocity) return;

        Vector2Int direction = GeneralUtilities.ClampVector2To8Direction(_rigidbody2D.velocity);

        if(currentFacingDirection != direction)
        {
            SetCurrentFacingDirection(direction);
        }
    }

    private void SetCurrentFacingDirection(Vector2Int facingDirection) => currentFacingDirection = facingDirection;

}
