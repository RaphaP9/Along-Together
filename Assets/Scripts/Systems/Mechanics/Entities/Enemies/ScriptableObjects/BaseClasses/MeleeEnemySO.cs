using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeEnemySO", menuName = "ScriptableObjects/Entities/Enemies/MeleeEnemy")]
public class MeleeEnemySO : EnemySO
{
    [Header("Melee Enemy Settings")]
    [Range(3f, 20f)] public float attackDistance;
    [Space]
    //RULE: chargingTimeMult + attackingTimeMult + postAttackMult = 1
    [Range(0f, 1f)] public float chargingTimeMult; //Charging Time = chargingTimeMult * 1/ AttackSpeed
    [Range(0f, 1f)] public float attackingTimeMult; //Attacking Time = attackingTimeMult * 1/ AttackSpeed
    [Range(0f, 1f)] public float recoverTimeMult; //PostAttack Time = postAttackTimeMult * 1/ AttackSpeed
    [Space]
    [Range(0f, 1f)] public float attackExecutionTimeMult; // Attack Execution on the middle of attack performance
    [Space]
    [Range(0f, 3f)] public float attackArea;
}
