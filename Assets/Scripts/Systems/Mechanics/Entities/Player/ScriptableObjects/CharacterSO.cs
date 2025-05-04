using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterSO", menuName = "ScriptableObjects/Entities/Character")]
public class CharacterSO : EntitySO, IDamageSourceSO
{
    [Header("Character Health Settings")]
    [Range(0, 10)] public int baseHealthRegen;
    [Range(0, 10)] public int baseShieldRegen;

    [Header("Character Damage Settings")]
    [Range(0, 10)] public int basicAttackDamage;
    [Range(0.5f, 3f)] public float basicAttackSpeed;
    [Space]
    [Range(0f, 1f)] public float baseAttackCritChance;
    [Range(0.5f, 2f)] public float baseAttackCritDamageMultiplier;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;

    #region IDamageSourceSO Methods
    public string GetDamageSourceName() => name;
    public Sprite GetDamageSourceSprite() => sprite;
    public string GetDamageSourceDescription() => description;
    public Color GetDamageSourceColor() => damageColor;
    #endregion
}
