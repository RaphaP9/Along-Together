using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    [Header("Ability Settings")]
    public int id;
    public string abilityName;
    public Sprite sprite;
    [TextArea(3,10)] public string description;
    public AbilityClassification abilityClassification;

    public abstract AbilityType GetAbilityType();
}
