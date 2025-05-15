using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : EntityAttack
{
    [Header("Enemy Attack Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;

    protected EnemySO EnemySO => enemyIdentifier.EnemySO;

    protected float timer;
    protected bool shouldAttack = false;
    protected bool shouldStopAttack = false;

    protected bool hasExecutedAttack = false;

    public void TriggerAttack() => shouldAttack = true;
    public void TriggerAttackStop() => shouldStopAttack = true;

    protected void ResetTimer() => timer = 0f;
    public abstract bool OnAttackExecution();
}
