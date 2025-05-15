using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRelativeHandler : MonoBehaviour
{
    [Header("RuntimeFilled")]
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private Vector2 directionToPlayer;
    [SerializeField] private float angleToPlayer;

    public float DistanceToPlayer => distanceToPlayer;
    public Vector2 DirectionToPlayer => directionToPlayer;
    public float AngleToPlayer => angleToPlayer;

    private void Update()
    {
        HandleRelativesToPlayer();
    }

    private void HandleRelativesToPlayer()
    {
        if (PlayerTransformRegister.Instance.PlayerTransform == null) return;

        distanceToPlayer = Vector2.Distance(GetPosition(), GetPlayerPosition());
        directionToPlayer = (GetPlayerPosition() - GetPosition()).normalized;
        angleToPlayer = GeneralUtilities.GetVector2AngleDegrees(directionToPlayer);
    }

    private Vector2 GetPlayerPosition()
    {
        return GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform);
    }

    private Vector2 GetPosition()
    {
        return GeneralUtilities.TransformPositionVector2(transform);
    }
}
