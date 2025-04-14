using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeStatModificationManager : AssetStatModificationManager<MovementTypeSO>
{
    public static MovementTypeStatModificationManager Instance { get; private set; }

    public static event EventHandler OnMovementTypeStatInitialized;
    public static event EventHandler OnMovementTypeStatUpdated;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementTypeStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override StatType GetStatType() => StatType.MovementType;

    protected override void InitializeStat()
    {
        OnMovementTypeStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMovementTypeStatUpdated?.Invoke(this, EventArgs.Empty);
    }

    public List<MovementTypeSO> ResolveMovementTypeStat(CharacterSO characterSO)
    {
        List<MovementTypeSO> resolvedMovementTypes = new List<MovementTypeSO>();

        if (temporalReplacementStatModifiers.Count > 0)
        {
            resolvedMovementTypes.Add(GetSpecificFromGeneric(temporalReplacementStatModifiers[^1].asset));
            return resolvedMovementTypes;
        }

        if (permanentReplacementStatModifiers.Count > 0)
        {
            resolvedMovementTypes.Add(GetSpecificFromGeneric(permanentReplacementStatModifiers[^1].asset));
            return resolvedMovementTypes;
        }

        foreach(MovementTypeSO movementType in characterSO.movementTypes)
        {
            resolvedMovementTypes.Add(movementType);
        }

        foreach(AssetStatModifier assetStatModifier in permanentUnionStatModifiers)
        {
            resolvedMovementTypes.Add(GetSpecificFromGeneric(assetStatModifier.asset));
        }

        foreach (AssetStatModifier assetStatModifier in temporalUnionStatModifiers)
        {
            resolvedMovementTypes.Add(GetSpecificFromGeneric(assetStatModifier.asset));
        }

        return resolvedMovementTypes;
    }
}
