using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStatusEffect
{
    [Header("Settings")]
    public string originGUID;

    public abstract EntityStatusEffectType GetEntityStatusEffectType();
}
