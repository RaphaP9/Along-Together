using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityWeaponAimHandler : MonoBehaviour
{
    [Header("Entity Components")]
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private EntityHealth entityHealth;
    [Space]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform refferenceAimPoint;

    [Header("Runtime Filled")]
    [SerializeField] private float pivotAimRefferenceAngle;
    [SerializeField] private float pivotAimRefferenceDistance;
    [Space]
    [SerializeField] private float pivotTargetAngle;
    [SerializeField] private float pivotTargetDistance;
    [Space]
    [SerializeField] private float pivotAngle;
    [SerializeField] private float weaponAimAngle;
    [SerializeField] private Vector2 weaponAimDirection;

    public float PivotAngle => pivotAngle;
    public float WeaponAimAngle => weaponAimAngle;
    public Vector2 WeaponAimDirection => weaponAimDirection;

    private void Start()
    {
        pivotAimRefferenceAngle = CalculatePivotAimRefferenceAngle();
        pivotAimRefferenceDistance = CalculatePivotAimRefferenceDistance();
    }

    private void LateUpdate()
    {
        HandlePivotRotation();
    }

    protected abstract Vector2 GetTargetPosition();

    private float CalculatePivotAimRefferenceAngle() => GeneralUtilities.GetVector2AngleDegrees(GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position));
    private float CalculatePivotAimRefferenceDistance() => GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position).magnitude;
    private float CalculatePivotTargetAngle() => GeneralUtilities.GetVector2AngleDegrees(GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position));
    private float CalculatePivotTargetDistance() => (GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position)).magnitude;

    private void HandlePivotRotation()
    {
        if (!entityHealth.IsAlive()) return;

        //Update Values

        pivotTargetAngle = CalculatePivotTargetAngle();
        pivotTargetDistance = CalculatePivotTargetDistance();

        //Components of the equation

        float phi = pivotAimRefferenceAngle;
        float AB = pivotAimRefferenceDistance;

        float alpha = pivotTargetAngle;
        float AC = pivotTargetDistance;

        float beta;

        if (AC <= AB)
        {
            return; //Aiming Inside Weapon
        }

        #region Pivot Angle Logic
        if (facingDirectionHandler.IsFacingRight()) //Take In count Scale Flip
        {
            beta = alpha - Mathf.Asin((AB / AC) * Mathf.Sin(phi * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
        }
        else
        {
            beta = -180 + alpha + Mathf.Asin((AB / AC) * Mathf.Sin(phi * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
        }

        pivotAngle = beta;

        UpdatePivotRotation();
        #endregion

        #region Aim Angle Logic

        weaponAimAngle = facingDirectionHandler.IsFacingRight() ? beta : 180 + beta;
        weaponAimDirection = GeneralUtilities.GetAngleDegreesVector2(weaponAimAngle);

        #endregion
    }

    private void UpdatePivotRotation()
    {
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, pivotAngle);
    }

}
