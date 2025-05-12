using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimDirectionerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("RuntimeFilled")]
    [SerializeField] private Vector2 aimDirection;
    [SerializeField] private float aimAngle;

    public Vector2 AimDirection => aimDirection;

    private void Update()
    {
        HandleAim();
    }

    private void HandleAim()
    {
        if (!CanAim()) return;

        aimDirection = mouseDirectionHandler.NormalizedMouseDirection;
        aimAngle = GeneralUtilities.GetVector2AngleDegrees(aimDirection);

        UpdateRotation(aimAngle);

    }

    private bool CanAim()
    {
        if (!playerHealth.IsAlive()) return false;

        return true;
    }

    private void UpdateRotation(float aimAngle)
    {
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    public bool IsAimingRight()
    {
        if (aimDirection.x >= 0) return true;
        return false;
    }
}
