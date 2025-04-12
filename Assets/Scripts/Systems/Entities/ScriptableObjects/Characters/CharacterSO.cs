using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Character")]
public class CharacterSO : EntitySO
{
    [Header("Character Stats Settings")]
    [Range(0, 10)] public int healthRegen;
    [Space]
    [Range(1f, 2f)] public float attackDamageMultiplier;
    [Space]
    [Range(0f, 1f)] public float attackCritChance;
    [Range(0.5f, 2f)] public float attackCritDamageMultiplier;

    [Header("Visual")]
    public Transform characterVisualTransform;
}
