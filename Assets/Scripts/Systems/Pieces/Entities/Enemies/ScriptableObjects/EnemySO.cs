using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySO", menuName = "ScriptableObjects/Entities/Enemy")]
public class EnemySO : EntitySO, IDamageSource
{
    [Header("Enemy Settings")]
    [Range(0, 10)] public int oreDrop;
    [Space]
    [Range(0f, 1f)] public float critChance;
    [Range(0f, 1f)] public float critDamageMultiplier;
    [Space]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;
    [Space]
    public Transform enemyPrefab;

    [Header("Damage Settings")]
    [ColorUsage(true, true)] public Color damageColor;

    public string GetName() => name;
    public Sprite GetSprite() => sprite;
    public string GetDescription() => description;
    public Color GetDamageColor() => damageColor;
}
