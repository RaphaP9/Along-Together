using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MaxHealthStatModificationManager : NumericStatModificationManager
{
    public static event EventHandler OnMaxHealthStatInitialized;
    public static event EventHandler OnMaxHealthStatUpdated;

    protected override StatType GetStatType() => StatType.MaxHealth;

    protected override void InitializeStat()
    {
        OnMaxHealthStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnMaxHealthStatUpdated?.Invoke(this, EventArgs.Empty);
    }
}
