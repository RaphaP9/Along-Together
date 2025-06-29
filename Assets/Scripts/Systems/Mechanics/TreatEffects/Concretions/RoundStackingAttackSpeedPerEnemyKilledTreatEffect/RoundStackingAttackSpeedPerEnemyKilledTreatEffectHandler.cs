using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStackingAttackSpeedPerEnemyKilledTreatEffectHandler : StackingTreatEffectHandler
{
    public static RoundStackingAttackSpeedPerEnemyKilledTreatEffectHandler Instance { get; private set; }

    private RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO => treatEffectSO as RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO;

    private void OnEnable()
    {
        EnemyHealth.OnAnyEnemyDeath += EnemyHealth_OnAnyEnemyDeath;
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        EnemyHealth.OnAnyEnemyDeath -= EnemyHealth_OnAnyEnemyDeath;
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTreatDeactivatedByInventoryObjectsMethod()
    {
        base.OnTreatDeactivatedByInventoryObjectsMethod();
        ResetStacks();
    }

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.refferencialGUID); //Remove Stats from previous stacked value
        TemporalNumericStatModifierManager.Instance.AddSingleNumericStatModifier(RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.refferencialGUID, GetStatPerStack(stacks));
    }

    protected override void ResetStacks()
    {
        base.ResetStacks();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.refferencialGUID);
    }

    private NumericEmbeddedStat GetStatPerStack(int stacks)
    {
        NumericEmbeddedStat stackedEmbeddedStat = new NumericEmbeddedStat
        {
            numericStatType = RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.statPerStack.numericStatType,
            numericStatModificationType = RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.statPerStack.numericStatModificationType,
            value = RoundStackingAttackSpeedPerEnemyKilledTreatEffectSO.statPerStack.value * stacks
        };

        return stackedEmbeddedStat;
    }


    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if (e.newState == GameManager.State.Combat)
        {
            isStacking = true;
            return;
        }

        if (e.previousState == GameManager.State.Combat)
        {
            ResetStacks();
            isStacking = false;
            return;
        }
    }


    private void EnemyHealth_OnAnyEnemyDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!isStacking) return;
        if (e.damageSource.GetDamageSourceClassification() != DamageSourceClassification.Character) return;
        AddStacks(1);
    }
}