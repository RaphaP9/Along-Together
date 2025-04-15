using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MovementTypeStatModificationManager : AssetStatModificationManager<MovementTypeSO>
{
    public static event EventHandler OnMovementTypeStatInitialized;
    public static event EventHandler OnMovementTypeStatUpdated;

    protected override StatType GetStatType() => StatType.MovementType;

    protected override void InitializeStat()
    {
        OnMovementTypeStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMovementTypeStatUpdated?.Invoke(this, EventArgs.Empty);
    }
}
