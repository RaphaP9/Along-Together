using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySO : ScriptableObject, IAttackableSO //All entities Have Health & Movement Stats
{
    [Header("Entity Identifiers")]
    public int id;
    public string entityName;
    [TextArea(3, 10)] public string description;
    public Sprite sprite;
    [ColorUsage(true, true)] public Color color;
    [Space]
    public Transform prefab;

    [Header("Entity Health Settings")]
    [Range(0, 20)] public int baseHealth;
    [Range(0, 20)] public int baseShield;
    [Range(0, 20)] public int baseArmor;
    [Space]
    [Range(0, 1)] public float baseDodgeChance;

    [Header("Entity Movement Settings")]
    [Range(0f, 10f)] public float baseMovementSpeed;

    #region IAttackableSO Methods
    public Color GetAttackableColor() => color;
    public string GetAttackableName() => entityName;
    public string GetAttackableDescription() => description;
    public Sprite GetAttackableSprite() => sprite;
    #endregion
}
