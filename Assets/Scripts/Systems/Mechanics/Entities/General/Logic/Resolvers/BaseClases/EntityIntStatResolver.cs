using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityIntStatResolver : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected EntitySO entitySO;

    [Header("Runtime Filled")]
    [SerializeField] protected int value;

    public int Value => value;

    public event EventHandler<OnStatEventArgs> OnEntityStatInitialized;
    public event EventHandler<OnStatEventArgs> OnEntityStatUpdated;

    public class OnStatEventArgs : EventArgs
    {
        public int value;
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

    protected abstract int CalculateStat();

    protected virtual void RecalculateStat()
    {
        value = CalculateStat();
        OnEntityStatUpdated?.Invoke(this, new OnStatEventArgs { value = value });
    }
}
