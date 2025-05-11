using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackPointHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("RuntimeFilled")]
    [SerializeField] private float distanceFromPlayerCenter;
    [SerializeField] private Vector2 aimDirection;
    [SerializeField] private float aimAngle;

    public Vector2 AimDirection => aimDirection;

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleAim();
    }

    private void InitializeVariables()
    {
        distanceFromPlayerCenter = Vector3.Distance(playerTransform.position, transform.position);
    }

    private void HandleAim()
    {
        if (!CanAim()) return;

        aimDirection = CalculateNormalizedAimDirection();
        aimAngle = GeneralUtilities.GetVector2AngleDegrees(aimDirection);

        UpdateAttackPointPosition(aimDirection);
        UpdateRotation(aimAngle);

    }

    private bool CanAim()
    {
        if (!playerHealth.IsAlive()) return false;

        return true;
    }

    private Vector2 CalculateNormalizedAimDirection()
    {
        Vector2 aimVector = ScreenInput.Instance.GetWorldMousePosition() - GeneralUtilities.TransformPositionVector2(playerTransform);
        aimVector.Normalize();

        return aimVector;
    }

    private void UpdateAttackPointPosition(Vector2 aimDirection)
    {
        Vector3 position = playerTransform.position + GeneralUtilities.Vector2ToVector3(aimDirection) * distanceFromPlayerCenter;
        transform.position = position;
    }

    private void UpdateRotation(float aimAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
}
