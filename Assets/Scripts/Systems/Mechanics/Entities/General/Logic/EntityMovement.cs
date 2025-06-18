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
    [SerializeField] protected List<Component> movementInterruptorComponents;

    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] protected float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] protected float smoothDirectionFactor = 5f;


    protected List<IMovementInterruptor> movementInterruptors;

    protected virtual void Awake()
    {
        movementInterruptors = GeneralUtilities.TryGetGenericsFromComponents<IMovementInterruptor>(movementInterruptorComponents);
    }

    protected float GetMovementSpeedValue() => entityMovementSpeedStatResolver.Value;
    public float GetCurrentSpeed() => _rigidbody2D.velocity.magnitude;

    protected bool CanApplyMovement()
    {
        foreach (IMovementInterruptor displacementAbility in movementInterruptors)
        {
            if (displacementAbility.IsDisplacing()) return false;
        }

        return true;
    }
}
