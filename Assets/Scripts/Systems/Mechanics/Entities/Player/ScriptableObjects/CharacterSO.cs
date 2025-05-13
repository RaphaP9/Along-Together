using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Character")]
public class CharacterSO : EntitySO
{
    [Header("Character Health Settings")]
    [Range(0, 10)] public int baseHealthRegen;
    [Range(0, 10)] public int baseShieldRegen;

    [Header("Character Cooldown Settings")]
    [Range(0f, 1f)] public float baseCooldownReduction;
}
