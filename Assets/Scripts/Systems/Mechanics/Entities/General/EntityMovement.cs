using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Header("Smooth Settings")]
    [SerializeField, Range(1f, 100f)] protected float smoothVelocityFactor = 5f;
    [SerializeField, Range(1f, 100f)] protected float smoothDirectionFactor = 5f;

    [Header("Runtime Filled")]
    [SerializeField] protected float movementSpeed;

    #region Events
    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsInitialized;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsInitialized;

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityStatsUpdated;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityStatsUpdated;

    //

    public static event EventHandler<OnEntityStatsEventArgs> OnAnyEntityMovementSpeedChanged;
    public event EventHandler<OnEntityStatsEventArgs> OnEntityMovementSpeedChanged;
    #endregion

    #region EventArgs Classes
    public class OnEntityStatsEventArgs : EventArgs
    {
        public float movementSpeed;
    }
    #endregion

    protected virtual void OnEnable()
    {
        StatModifierManager.OnStatModifierManagerUpdated += StatModifierManager_OnStatModifierManagerUpdated;
    }

    protected virtual void OnDisable()
    {
        StatModifierManager.OnStatModifierManagerUpdated -= StatModifierManager_OnStatModifierManagerUpdated;
    }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        movementSpeed = CalculateMovementSpeed();

        OnEntityStatsInitializedMethod();
    }

    protected abstract float CalculateMovementSpeed();

    protected virtual void CheckMovementSpeedChanged()
    {
        float previousMovementSpeed = movementSpeed;
        float newMovementSpeed = CalculateMovementSpeed();

        if (previousMovementSpeed != newMovementSpeed)
        {
            movementSpeed = newMovementSpeed;
            OnEntityMovementSpeedChangedMethod();
        }
    }

    #region Virtual Event Methods
    
    protected virtual void OnEntityStatsInitializedMethod()
    {
        OnEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnAnyEntityStatsInitialized?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }

    protected virtual void OnEntityStatsUpdatedMethod()
    {
        OnEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnAnyEntityStatsUpdated?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }

    protected virtual void OnEntityMovementSpeedChangedMethod()
    {
        OnEntityMovementSpeedChanged?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
        OnAnyEntityMovementSpeedChanged?.Invoke(this, new OnEntityStatsEventArgs { movementSpeed = CalculateMovementSpeed() });
    }
    #endregion

    private void StatModifierManager_OnStatModifierManagerUpdated(object sender, EventArgs e)
    {
        CheckMovementSpeedChanged();
    }
}
