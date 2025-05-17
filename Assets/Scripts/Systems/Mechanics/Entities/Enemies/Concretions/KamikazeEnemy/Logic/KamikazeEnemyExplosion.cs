using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeleeEnemyAttack;

public class KamikazeEnemyExplosion : EnemyExplosion
{
    [Header("States - Runtime Filled")]
    [SerializeField] protected KamikazeExplosionState kamikazeExplosionState;

    protected enum KamikazeExplosionState {NotExploding, Charging} //Kamikaze Enemies Only have 2 states
    private KamikazeEnemySO KamikazeEnemySO => EnemySO as KamikazeEnemySO;

    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyCharge;
    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyExplosion;
    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyStopExploding;

    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyCharge;
    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyExplosion;
    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyStopExploding;

    public class OnKamikazeEnemyExplosionEventArgs : OnEnemyExplosionEventArgs
    {
        public int explosionDamage;
    }

    private void Start()
    {
        ResetTimer();
        SetKamikazeAtackState(KamikazeExplosionState.NotExploding);
    }

    private void Update()
    {
        HandleMeleeAttack();
    }

    #region Logic
    private void HandleMeleeAttack()
    {
        switch (kamikazeExplosionState)
        {
            case KamikazeExplosionState.NotExploding:
            default:
                NotExplodingLogic();
                break;
            case KamikazeExplosionState.Charging:
                ChargingLogic();
                break;
        }
    }

    private void NotExplodingLogic()
    {

    }

    private void ChargingLogic()
    {

    }

    private void TransitionToState(KamikazeExplosionState state)
    {
        switch (state)
        {
            case KamikazeExplosionState.NotExploding:
                SetKamikazeAtackState(KamikazeExplosionState.NotExploding);
                OnAnyKamikazeEnemyStopExploding?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                OnKamikazeEnemyStopExploding?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                break;
            case KamikazeExplosionState.Charging:
                SetKamikazeAtackState(KamikazeExplosionState.Charging);
                OnAnyKamikazeEnemyCharge?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                OnKamikazeEnemyCharge?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                break;
        }

        ResetTimer();
    }

    #endregion

    private void SetKamikazeAtackState(KamikazeExplosionState state) => kamikazeExplosionState = state;

    public override bool OnExplosionExecution() => kamikazeExplosionState != KamikazeExplosionState.NotExploding;

    #region Virtual Event Methods
    protected override void OnEnemyExplosionMethod()
    {
        base.OnEnemyExplosionMethod();

        OnAnyKamikazeEnemyExplosion?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
        OnKamikazeEnemyExplosion?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { enemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
    }
    #endregion
}
