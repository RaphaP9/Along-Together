using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStatusEffect
{
    [Header("Settings")]
    public string originGUID;
    [Range(0.5f, 10f)] public float duration;

    public abstract EntityStatusEffectType GetEntityStatusEffectType();
}
