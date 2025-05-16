using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Header("Entity Movement Components")]
    [SerializeField] protected Rigidbody2D _rigidbody2D;
    [SerializeField] protected EntityMovementSpeedStatResolver entityMovementSpeedStatResolver;
    [Space]
    [SerializeField] protected List<Component> displacementComponents;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] protected float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] protected float smoothDirectionFactor = 5f;


    protected List<IDisplacement> displacements;

    protected virtual void Awake()
    {
        GetDisplacementInterfaces();
    }

    private void GetDisplacementInterfaces() => displacements = GeneralUtilities.TryGetGenericsFromComponents<IDisplacement>(displacementComponents);
    protected float GetMovementSpeedValue() => entityMovementSpeedStatResolver.Value;
    public float GetCurrentSpeed() => _rigidbody2D.velocity.magnitude;
}
