using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemyExplosion : EnemyExplosion
{
    [Header("States - Runtime Filled")]
    [SerializeField] protected KamikazeExplosionState kamikazeExplosionState;

    protected enum KamikazeExplosionState {NotExploding, Charging} //Kamikaze Enemies Only have 2 states
    private KamikazeEnemySO KamikazeEnemySO => EnemySO as KamikazeEnemySO;

    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyCharge;
    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyStopExploding;

    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyCharge;
    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyStopExploding;

    public static event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnAnyKamikazeEnemyExplosion;
    public event EventHandler<OnKamikazeEnemyExplosionEventArgs> OnKamikazeEnemyExplosion;

    public class OnKamikazeEnemyExplosionEventArgs : EventArgs
    {
        public KamikazeEnemySO kamikazeEnemySO;
        public List<Transform> explosionPoints;
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
        hasExecutedExplosion = false;

        if (!CanExplode())
        {
            ResetTimer();
            return;
        }

        if (shouldExplode)
        {
            shouldExplode = false;
            TransitionToState(KamikazeExplosionState.Charging);
        }
    }

    private void ChargingLogic()
    {
        if (shouldStopExplosion)
        {
            shouldStopExplosion = false;
            TransitionToState(KamikazeExplosionState.NotExploding);
            return;
        }

        if (timer < GetExplosionTime())
        {
            timer += Time.deltaTime;
            return;
        }

        hasExecutedExplosion = true;

        OnEntityExplosionCompletedMethod();
        Explode();

        TransitionToState(KamikazeExplosionState.NotExploding);
    }

    private void TransitionToState(KamikazeExplosionState state)
    {
        switch (state)
        {
            case KamikazeExplosionState.NotExploding:
                SetKamikazeAtackState(KamikazeExplosionState.NotExploding);
                OnAnyKamikazeEnemyStopExploding?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                OnKamikazeEnemyStopExploding?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                break;
            case KamikazeExplosionState.Charging:
                SetKamikazeAtackState(KamikazeExplosionState.Charging);
                OnAnyKamikazeEnemyCharge?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                OnKamikazeEnemyCharge?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
                break;
        }

        ResetTimer();
    }

    #endregion

    private void SetKamikazeAtackState(KamikazeExplosionState state) => kamikazeExplosionState = state;
    private float GetExplosionTime() => KamikazeEnemySO.explosionTime;

    public override bool OnExplosionExecution() => kamikazeExplosionState != KamikazeExplosionState.NotExploding;

    protected override void Explode()
    {
        bool isCrit = true;
        int damage = KamikazeEnemySO.explosionDamage;
;
        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(explosionPoints);

        DamageData damageData = new DamageData { damage = damage, isCrit = isCrit, damageSource = KamikazeEnemySO, canBeDodged = false, canBeImmuned = true };

        MechanicsUtilities.DealDamageInAreas(positions, KamikazeEnemySO.explosionRadius, damageData, explosionLayermask, new List<Transform> { transform });
        entityHealth.Excecute(KamikazeEnemySO); 

        OnEntityExplosionMethod(damage);
    }

    #region Virtual Event Methods
    protected override void OnEntityExplosionCompletedMethod()
    {
        base.OnEntityExplosionCompletedMethod();

        OnAnyKamikazeEnemyExplosion?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
        OnKamikazeEnemyExplosion?.Invoke(this, new OnKamikazeEnemyExplosionEventArgs { kamikazeEnemySO = KamikazeEnemySO, explosionPoints = explosionPoints, explosionDamage = KamikazeEnemySO.explosionDamage });
    }
    #endregion

    #region Subscriptions
    protected override void EntityHealth_OnEntityDeath(object sender, EventArgs e)
    {
        if (hasExecutedExplosion) return;
        if (explodeOnDeath) Explode();
    }
    #endregion
}
