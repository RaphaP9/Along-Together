using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeleeEnemyAttack : EnemyAttack
{
    [Header("Melee Enemy Attack Components")]
    [SerializeField] protected List<Transform> attackPoints;

    [Header("States")]
    [SerializeField] protected MeleeAttackState meleeAttackState;

    protected enum MeleeAttackState { NotAttacking, Charging, Attacking, Recovering }
    private MeleeEnemySO MeleeEnemySO => EnemySO as MeleeEnemySO;

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
        hasExecutedAttack = false;

        if (!CanAttack())
        {
            ResetTimer();
            return;
        }

        if (shouldAttack)
        {
            shouldAttack = false;

            TransitionToState(MeleeAttackState.Charging);
        }
    }

    private void ChargingLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(MeleeAttackState.NotAttacking);
            return;
        }

        if (timer < GetChargingTime())
        {
            timer += Time.deltaTime;
            return;
        }

        TransitionToState(MeleeAttackState.Attacking);
    }

    private void AttackingLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(MeleeAttackState.NotAttacking);
            return;
        }

        if(timer >= GetAttackExecutionTime() && !hasExecutedAttack) //Control when to trigger the attack relative to the AttackState
        {
            Attack();
            hasExecutedAttack = true;
        }

        if (timer < GetAttackingTime())
        {
            timer += Time.deltaTime;
            return;
        }

        TransitionToState(MeleeAttackState.Recovering);
    }

    private void RecoveringLogic()
    {
        if (shouldStopAttack)
        {
            shouldStopAttack = false;
            TransitionToState(MeleeAttackState.NotAttacking);
            return;
        }

        if (timer < GetRecoverTime())
        {
            timer += Time.deltaTime;
            return;
        }

        hasExecutedAttack = false;

        if (shouldAttack) TransitionToState(MeleeAttackState.Charging);
        else TransitionToState(MeleeAttackState.NotAttacking);
    }

    private void SetMeleeAttackState(MeleeAttackState state) => meleeAttackState = state;

    private void TransitionToState(MeleeAttackState state)
    {
        switch (state)
        {
            case MeleeAttackState.NotAttacking:
                SetMeleeAttackState(MeleeAttackState.NotAttacking);
                OnAnyMeleeEnemyStopAttacking?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                OnMeleeEnemyStopAttacking?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                break;
            case MeleeAttackState.Charging:
                SetMeleeAttackState(MeleeAttackState.Charging);
                OnAnyMeleeEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                OnMeleeEnemyCharge?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                break;
            case MeleeAttackState.Attacking:
                SetMeleeAttackState(MeleeAttackState.Attacking);
                OnAnyMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                OnMeleeEnemyAttack?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                break;
            case MeleeAttackState.Recovering:
                SetMeleeAttackState(MeleeAttackState.Recovering);
                OnAnyMeleeEnemyRecover?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                OnMeleeEnemyRecover?.Invoke(this, new OnEnemyAttackEventArgs { meleeEnemySO = MeleeEnemySO, attackPoints = attackPoints });
                break;
        }

        ResetTimer();
    }

    private float GetChargingTime() => 1 / GetRevisedAttackSpeed() * MeleeEnemySO.GetNormalizedChargingMult();
    private float GetAttackingTime() => 1 / GetRevisedAttackSpeed() * MeleeEnemySO.GetNormalizedAttackingMult();
    private float GetRecoverTime() => 1 / GetRevisedAttackSpeed() * MeleeEnemySO.GetNormalizedRecoverMult();
    private float GetAttackExecutionTime() => GetRevisedAttackSpeed() * Mathf.Clamp01(MeleeEnemySO.attackExecutionTimeMult);

    public override bool OnAttackExecution() => meleeAttackState != MeleeAttackState.NotAttacking;

    protected override void Attack()
    {
        bool isCrit = MechanicsUtilities.EvaluateCritAttack(entityAttackCritChanceStatResolver.Value);
        int damage = isCrit ? MechanicsUtilities.CalculateCritDamage(entityAttackDamageStatResolver.Value, entityAttackCritDamageMultiplierStatResolver.Value) : entityAttackDamageStatResolver.Value;

        List<Vector2> positions = GeneralUtilities.TransformPositionVector2List(attackPoints);

        DamageData damageData = new DamageData { damage = damage, isCrit = isCrit, damageSource = MeleeEnemySO, canBeDodged = true, canBeImmuned = true };

        MechanicsUtilities.DealDamageInAreas(positions, MeleeEnemySO.attackArea, damageData, attackLayermask, new List<Transform> { transform });

        OnEntityAttackMethod(isCrit, damage);
    }
}
