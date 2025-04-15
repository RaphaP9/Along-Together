using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArmorStatModificationManager : NumericStatModificationManager
{
    public static event EventHandler OnArmorStatInitialized;
    public static event EventHandler OnArmorStatUpdated;

    protected override StatType GetStatType() => StatType.Armor;

    protected override void InitializeStat()
    {
        OnArmorStatInitialized?.Invoke(this, EventArgs.Empty);
    }

    protected override void UpdateStat()
    {
        OnArmorStatUpdated?.Invoke(this, EventArgs.Empty);
    }
}
