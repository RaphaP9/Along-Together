using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponPivotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityAimDirectionerHandler aimDirectionerHandler;
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private Transform refferenceAimPoint;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 pivotAimRefferenceOffset;
    [SerializeField] private float pivotAimRefferenceAngle;
    [Space]
    [SerializeField] private float aimAngle;
    [SerializeField] private float pivotAngle;

    public float PivotAngle => pivotAngle;

    private void Start()
    {
        CalculatePivotAimRefferenceOffset();
    }

    private void Update()
    {
        HandlePivotRotation();
    }

    private void CalculatePivotAimRefferenceOffset()
    {
        pivotAimRefferenceOffset = refferenceAimPoint.localPosition;
        pivotAimRefferenceAngle = GeneralUtilities.GetAngleDegreesVector2(pivotAimRefferenceOffset);
    }

    private void HandlePivotRotation()
    {
        if (!entityHealth.IsAlive()) return;

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        aimAngle = aimDirectionerHandler.AimAngle;
    }
}
