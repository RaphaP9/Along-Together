using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : EntityAttack
{
    [Header("Enemy Attack Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    protected EnemySO EnemySO => enemyIdentifier.EnemySO;
}
