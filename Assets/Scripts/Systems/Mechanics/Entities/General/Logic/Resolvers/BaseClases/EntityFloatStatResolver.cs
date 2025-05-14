using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityFloatStatResolver : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntityIdentifier entityIdentifier;

    [Header("Runtime Filled")]
    [SerializeField] protected float value;

    public float Value => value;

    public event EventHandler<OnStatEventArgs> OnEntityStatInitialized;
    public event EventHandler<OnStatEventArgs> OnEntityStatUpdated;

    public class OnStatEventArgs : EventArgs
    {
        public float value;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        value = CalculateStat();
        OnEntityStatInitialized?.Invoke(this, new OnStatEventArgs { value = value });
    }

    protected abstract float CalculateStat();

    protected virtual void RecalculateStat()
    {
        value = CalculateStat();
        OnEntityStatUpdated?.Invoke(this, new OnStatEventArgs { value = value });
    }
}
