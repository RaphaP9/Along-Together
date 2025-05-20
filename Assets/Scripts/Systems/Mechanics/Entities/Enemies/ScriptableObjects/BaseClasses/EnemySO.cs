using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySO", menuName = "ScriptableObjects/Entities/Enemies/Enemy(Default)")]
public class EnemySO : EntitySO, IOreSourceSO
{
    [Header("Enemy Extra Settings")]
    [Range(0, 10)] public int oreDrop;
    [Space]
    [Range(1f, 5f)] public float spawnDuration;
    [Range(1f, 10f)] public float cleanupTime;

    #region IOreSourceSO Methods
    public Color GetOreSourceColor() => color;
    public string GetOreSourceName() => entityName;
    public string GetOreSourceDescription() => description;
    public Sprite GetOreSourceSprite() => sprite;
    #endregion
}
