using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityExplosion : MonoBehaviour
{
    [Header("Entity Explosion Components")]
    [SerializeField] protected EntityHealth entityHealth;
    [SerializeField] protected List<Transform> explosionPoints;

    [Header("Entity Explosion Settings")]
    [SerializeField] protected LayerMask explosionLayermask;

    protected bool hasExecutedExplosion = false;


    #region Events
    public event EventHandler<OnEntityExplosionEventArgs> OnEntityExplosion;
    public static event EventHandler<OnEntityExplosionEventArgs> OnAnyEntityExplosion;

    public event EventHandler<OnEntityExplosionCompletedEventArgs> OnAnyEntityExplosionCompleted;
    public static event EventHandler<OnEntityExplosionCompletedEventArgs> OnEntityExplosionCompleted;
    #endregion

    #region EventArgs Classes
    public class OnEntityExplosionEventArgs
    {
        public List<Transform> explosionPoints;
        public int explosionDamage;
    }

    public class OnEntityExplosionCompletedEventArgs
    {

    }
    #endregion

    protected virtual bool CanExplode()
    {
        if (!entityHealth.IsAlive()) return false;

        return true;
    }

    public abstract bool OnExplosionExecution();

    #region Virtual Event Methods
    protected virtual void OnEntityExplosionMethod(int explosionDamage)
    {
        OnEntityExplosion?.Invoke(this, new OnEntityExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage });
        OnAnyEntityExplosion?.Invoke(this, new OnEntityExplosionEventArgs { explosionPoints = explosionPoints, explosionDamage = explosionDamage });
    }

    protected virtual void OnEntityExplosionCompletedMethod()
    {
        OnEntityExplosionCompleted?.Invoke(this, new OnEntityExplosionCompletedEventArgs { });
        OnAnyEntityExplosionCompleted?.Invoke(this, new OnEntityExplosionCompletedEventArgs { });
    }
    #endregion
}
