using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EmbeddedStat
{
    public StatType statType;

    public abstract StatValueType GetStatValueType();
}