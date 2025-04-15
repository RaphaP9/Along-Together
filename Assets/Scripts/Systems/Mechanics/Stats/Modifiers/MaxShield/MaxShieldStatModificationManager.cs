using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MaxShieldStatModificationManager : NumericStatModificationManager
{
    public static event EventHandler OnMaxShieldStatInitialized;
    public static event EventHandler OnMaxShieldStatUpdated;

    protected override StatType GetStatType() => StatType.MaxShield;

    protected override void InitializeStat()
    {
        OnMaxShieldStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMaxShieldStatUpdated?.Invoke(this, EventArgs.Empty);
    }
}
