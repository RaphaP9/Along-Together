using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicDashSO", menuName = "ScriptableObjects/Abilities/Test/BasicDash")]
public class BasicDashSO : AbilitySO, IActiveAbilitySO
{
    [Header("Active Ability Settings")]
    [Range(0.5f, 100f)] public float baseCooldown;

    [Header("Specific Settings")]
    [Range(1f, 12f)] public float dashDistance;
    [Range(0.1f, 1f)] public float dashTime;
    [Range(0f, 50f)] public float dashResistance;


    public float GetBaseCooldown() => baseCooldown;
}
