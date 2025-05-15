using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/MeleeEnemy")]
public class MeleeEnemySO : EnemySO
{
    [Header("Melee Enemy Settings")]
    [Range(3f, 20f)] public float attackDistance;
    [Space]
    [Range(0f, 3f)] public float chargingTime;
    [Range(0f, 3f)] public float attackingTime;
    [Range(0f, 3f)] public float postAttackTime;
    [Space]
    [Range(0f, 3f)] public float attackArea;
}
