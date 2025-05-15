using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Header("Entity Movement Components")]
    [SerializeField] protected EntityMovementSpeedStatResolver entityMovementSpeedStatResolver;
    [Space]
    [SerializeField] protected List<Component> displacementComponents;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] protected float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] protected float smoothDirectionFactor = 5f;

    protected Rigidbody2D _rigidbody2D;

    protected List<IDisplacement> displacements;

    protected virtual void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        GetDisplacementInterfaces();
    }

    private void GetDisplacementInterfaces() => displacements = GeneralUtilities.TryGetGenericsFromComponents<IDisplacement>(displacementComponents);

    protected float GetMovementSpeedValue() => entityMovementSpeedStatResolver.Value;
}
