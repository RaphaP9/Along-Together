using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Characters/Character(Default)")]
public class CharacterSO : EntitySO
{
    [Header("Character Health Settings")]
    [Range(0, 10)] public int baseHealthRegen;
    [Range(0, 10)] public int baseShieldRegen;

    [Header("Character Cooldown Settings")]
    [Range(0f, 1f)] public float baseCooldownReduction;

    [Header("Character Stats Affinity")]
    public List<NumericStatType> statAffinities;

    public override DamageSourceClassification GetDamageSourceClassification() => DamageSourceClassification.Character;
}
