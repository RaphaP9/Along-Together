using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponPivotHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private Transform refferenceAimPoint;

    [Header("Runtime Filled")]
    [SerializeField] private Vector2 pivotAimRefferenceOffset;
    [Space]
    [SerializeField] private float refferencedAimAngle;

    public float RefferencedAimAngle => refferencedAimAngle;

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
        pivotAimRefferenceOffset = refferenceAimPoint.position - transform.position;
    }

    private void HandlePivotRotation()
    {
        if (!entityHealth.IsAlive()) return;

        UpdateRotation();
    }

    private void UpdateRotation()
    {

    }
}
