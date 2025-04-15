using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatModificationManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected void Awake()
    {
        SetSingleton();
    }

    protected virtual void Start()
    {
        LoadRuntimeData();
        InitializeStat();
    }

    protected abstract void SetSingleton();
    protected abstract void LoadRuntimeData();
    protected abstract void InitializeStat();
    protected abstract void UpdateStat();

    public abstract bool HasStatModifiers();
    public abstract int GetStatModifiersQuantity();

    protected abstract StatType GetStatType();
    protected abstract StatValueType GetStatValueType();

    public abstract void AddStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder);
    public abstract void RemoveStatModifiersByGUID(string originGUID);

    #region Subscriptions

    #endregion
}
