using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyExplosion : MonoBehaviour
{
    [Header("Enemy Explosion Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] protected List<Transform> explosionPoints;

    protected EnemySO EnemySO => enemyIdentifier.EnemySO;

    protected float timer;
    protected bool shouldExplode = false;
    protected bool shouldStopExplosion = false;

    protected bool hasExecutedExplosion = false;

    #region Events
    public event EventHandler<OnEnemyExplosionEventArgs> OnEnemyExplosion;
    public static event EventHandler<OnEnemyExplosionEventArgs> OnAnyEnemyExplosion;

    public event EventHandler<OnEnemyAttackCompletedEventArgs> OnAnyEnemyExplosionCompleted;
    public static event EventHandler<OnEnemyAttackCompletedEventArgs> OnEnemyExplosionCompleted;
    #endregion

    #region EventArgs Classes
    public class OnEnemyExplosionEventArgs
    {
        public EnemySO enemySO;
        public List<Transform> explosionPoints;
    }

    public class OnEnemyAttackCompletedEventArgs
    {
        public EnemySO enemySO;
    }
    #endregion

    public void TriggerExplosion() => shouldExplode = true;
    public void TriggerExplosionStop() => shouldStopExplosion = true;

    protected void ResetTimer() => timer = 0f;
    public abstract bool OnExplosionExecution();

    #region Virtual Event Methods
    protected virtual void OnEnemyExplosionMethod()
    {
        OnEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, explosionPoints = explosionPoints});
        OnAnyEnemyExplosion?.Invoke(this, new OnEnemyExplosionEventArgs { enemySO = enemyIdentifier.EnemySO, explosionPoints = explosionPoints });
    }

    protected virtual void OnEntityAttackCompletedMethod()
    {
        OnEnemyExplosionCompleted?.Invoke(this, new OnEnemyAttackCompletedEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnAnyEnemyExplosionCompleted?.Invoke(this, new OnEnemyAttackCompletedEventArgs { enemySO = enemyIdentifier.EnemySO });
    }
    #endregion
}
