using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponPivotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private MouseDirectionHandler mouseDirectionHandler;
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

    public float PivotAngle => pivotAngle;

    private void Start()
    {
        pivotAimRefferenceAngle = CalculatePivotAimRefferenceAngle();
        pivotAimRefferenceDistance = CalculatePivotAimRefferenceDistance();
    }

    private void Update()
    {
        HandlePivotRotation();
    }

    private float CalculatePivotAimRefferenceAngle() => GeneralUtilities.GetVector2AngleDegrees(GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position));
    private float CalculatePivotAimRefferenceDistance() => GeneralUtilities.SupressZComponent(refferenceAimPoint.position - weaponPivot.position).magnitude;
    private float CalculatePivotTargetAngle() => GeneralUtilities.GetVector2AngleDegrees(GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position));
    private float CalculatePivotTargetDistance() => (GetTargetPosition() - GeneralUtilities.SupressZComponent(weaponPivot.position)).magnitude;


    private Vector2 GetTargetPosition() => mouseDirectionHandler.Input;


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

        float beta = alpha - Mathf.Asin((AB/AC)*Mathf.Sin(phi*Mathf.Deg2Rad));

        pivotAngle = beta;

        UpdatePivotRotation();
    }

    private void UpdatePivotRotation()
    {
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, pivotAngle);
    }

}
