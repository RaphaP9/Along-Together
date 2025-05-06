using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySO : ScriptableObject
{
    [Header("Settings")]
    [SerializeField] private string abilityName;
    [SerializeField] private Sprite sprite;
    [Space]
    [SerializeField] AbilityType abilityType;
}
