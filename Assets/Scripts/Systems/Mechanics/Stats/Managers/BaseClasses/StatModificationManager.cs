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

    protected virtual void Start()
    {
        InitializeStat();
    }

    protected abstract void SetSingleton();
    protected abstract void InitializeStat();
    protected abstract void UpdateStat();

    public abstract bool HasPermanentStatModifiers();
    public abstract bool HasTemporalStatModifiers();
    public abstract int GetPermanentStatModifiersQuantity();
    public abstract int GetTemporalStatModifiersQuantity();
    public bool HasStatModifiers() => GetStatModifiersQuantity()>0;
    public int GetStatModifiersQuantity() => GetPermanentStatModifiersQuantity() + GetTemporalStatModifiersQuantity();


    protected abstract StatType GetStatType();
    protected abstract StatValueType GetStatValueType();

    public abstract void AddPermanentStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder);
    public abstract void RemovePermanentStatModifiersByGUID(string originGUID);

    public abstract void AddTemporalStatModifiers(string originGUID, IHasEmbeddedStats embeddedStatsHolder);
    public abstract void RemoveTemporalStatModifiersByGUID(string originGUID);

    #region Subscriptions

    #endregion
}
