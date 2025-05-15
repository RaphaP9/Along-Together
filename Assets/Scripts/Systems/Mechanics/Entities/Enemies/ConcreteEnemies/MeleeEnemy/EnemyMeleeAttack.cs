using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMeleeAttack : EnemyAttack
{
    [Header("Melee Enemy Attack Components")]
    [SerializeField] protected List<Transform> attackPoints;

    [Header("States")]
    [SerializeField] protected MeleeAttackState meleeAttackState;

    protected enum MeleeAttackState { NotAttacking, Charging, Attacking, Recovering }
    private MeleeEnemySO MeleeEnemySO => EnemySO as MeleeEnemySO;

    private float timer;
    private bool shouldAttack = false;
    private bool shouldStopAttack = false;

    public static event EventHandler<OnEnemyAttackEventArgs> OnAnyMeleeEnemyCharge;
    public static event EventHandler<OnEnemyAttackEventArgs> OnAnyMeleeEnemyAttack;
    public static event EventHandler<OnEnemyAttackEventArgs> OnAnyMeleeEnemyRecover;
    public static event EventHandler<OnEnemyAttackEventArgs> OnAnyMeleeEnemyStopAttacking;

    public event EventHandler<OnEnemyAttackEventArgs> OnMeleeEnemyCharge;
    public event EventHandler<OnEnemyAttackEventArgs> OnMeleeEnemyAttack;
    public event EventHandler<OnEnemyAttackEventArgs> OnMeleeEnemyRecover;
    public event EventHandler<OnEnemyAttackEventArgs> OnMeleeEnemyStopAttacking;

    public class OnEnemyAttackEventArgs : EventArgs
    {
        public MeleeEnemySO meleeEnemySO;
        public List<Transform> attackPoints;
    }

    private void Start()
    {
        ResetTimer();
        SetMeleeAttackState(MeleeAttackState.NotAttacking);
    }

    private void Update()
    {
        HandleMeleeAttack();
    }

    private void HandleMeleeAttack()
    {
        switch (meleeAttackState)
        {
            case MeleeAttackState.NotAttacking:
            default:
                NotAttackingLogic();
                break;
            case MeleeAttackState.Charging:
                ChargingLogic();
                break;
            case MeleeAttackState.Attacking:
                AttackingLogic();
                break;
            case MeleeAttackState.Recovering:
                RecoveringLogic();
                break;
        }
    }

    private void NotAttackingLogic()
    {
        if (!CanAttack())
        {
            ResetTimer();
            return;
        }

        if (shouldAttack)
        {
            SetMeleeAttackState(MeleeAttackState.Charging);
            OnAnyMeleeEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
            OnMeleeEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        }

        ResetTimer();
    }

    private void ChargingLogic()
    {
        if (timer < GetAttackChargeTime())
        {
            if (shouldStopAttack)
            {
                SetMeleeAttackState(MeleeAttackState.NotAttacking);

                OnAnyMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                OnMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });

                ResetTimer();
                return;
            }

            timer += Time.deltaTime;
            return;
        }

        Attack();
        SetMeleeAttackState(MeleeAttackState.Attacking);
        ResetTimer();
    }

    private void AttackingLogic()
    {

    }

    private void RecoveringLogic()
    {

    }

    private void SetMeleeAttackState(MeleeAttackState state) => meleeAttackState = state;

    private float GetAttackChargeTime() => GetAttackSpeed() * MeleeEnemySO.chargingTimeMult;
    private float GetAttackingTime() => GetAttackSpeed() * MeleeEnemySO.attackingTimeMult;
    private float GetAttackRecoverTime() => GetAttackSpeed() * MeleeEnemySO.recoverTimeMult;
    private void ResetTimer() => timer = 0f;

    public void TriggerAttack() => shouldAttack = true;
    public void TriggerAttackStop() => shouldStopAttack = true;

    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(entityAttackCritChanceStatResolver.Value);
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(entityAttackDamageStatResolver.Value, entityAttackCritDamageMultiplierStatResolver.Value) : entityAttackDamageStatResolver.Value;

        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        DamageData damageData = new DamageData { damage = damage, isCrit = isCrit, damageSource = MeleeEnemySO, canBeDodged = true, canBeImmuned = true };

        MechanicsUtilities.DealDamageInAreas(positions, MeleeEnemySO.attackArea, damageData, attackLayermask, new List<Transform> { transform });

        OnEntityAttackMethod(isCrit, damage);
    }

    protected override void OnEntityAttackMethod(bool isCrit, int attackDamage)
    {
        base.OnEntityAttackMethod(isCrit, attackDamage);

        OnAnyMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
        OnMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
    }
}
