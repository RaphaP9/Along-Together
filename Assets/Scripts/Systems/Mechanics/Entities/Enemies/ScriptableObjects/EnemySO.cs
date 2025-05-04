using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySO", menuName = "ScriptableObjects/Entities/Enemy")]
public class EnemySO : EntitySO, IDamageSourceSO, IOreSourceSO
{
    [Header("Enemy Damage Settings")]
    [Range(0, 10)] public int basicAttackDamage;
    [Range(0.5f, 3f)] public float basicAttackSpeed;
    [Space]
    [Range(0f, 1f)] public float baseAttackCritChance;
    [Range(0f, 1f)] public float baseAttackCritDamageMultiplier;
    [Space]
    [ColorUsage(true, true)] public Color damageColor;

    [Header("Enemy Extra Settings")]
    [Range(0, 10)] public int oreDrop;
    [Space]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;

    #region IDamageSourceSO Methods
    public string GetDamageSourceName() => entityName;
    public Sprite GetDamageSourceSprite() => sprite;
    public string GetDamageSourceDescription() => description;
    public Color GetDamageSourceColor() => damageColor;
    #endregion

    #region IOreSourceSO Methods
    public Color GetOreSourceColor() => color;
    public string GetOreSourceName() => entityName;
    public string GetOreSourceDescription() => description;
    public Sprite GetOreSourceSprite() => sprite;
    #endregion
}
