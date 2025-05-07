using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    [Header("ABility Settings")]
    public string abilityName;
    public Sprite sprite;
    [TextArea(3,10)] public string description;
    [Space]
    public AbilityType abilityType;
}
