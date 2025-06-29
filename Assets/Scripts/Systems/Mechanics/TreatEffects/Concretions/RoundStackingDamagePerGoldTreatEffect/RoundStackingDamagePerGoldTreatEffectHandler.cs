using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundStackingDamagePerGoldTreatEffectHandler : StackingTreatEffectHandler
{
    public static RoundStackingDamagePerGoldTreatEffectHandler Instance { get; private set; }

    private RoundStackingDamagePerGoldTreatEffectSO RoundStackingDamagePerGoldTreatEffectSO => treatEffectSO as RoundStackingDamagePerGoldTreatEffectSO;

    private void OnEnable()
    {
        GoldCollection.OnAnyGoldCollected += GoldCollection_OnAnyGoldCollected;
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    private void OnDisable()
    {
        GoldCollection.OnAnyGoldCollected -= GoldCollection_OnAnyGoldCollected;
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
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(RoundStackingDamagePerGoldTreatEffectSO.refferencialGUID); //Remove Stats from previous stacked value
        TemporalNumericStatModifierManager.Instance.AddSingleNumericStatModifier(RoundStackingDamagePerGoldTreatEffectSO.refferencialGUID, GetStatPerStack(stacks));
    }

    protected override void ResetStacks()
    {
        base.ResetStacks();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(RoundStackingDamagePerGoldTreatEffectSO.refferencialGUID);
    }

    private NumericEmbeddedStat GetStatPerStack(int stacks)
    {
        NumericEmbeddedStat stackedEmbeddedStat = new NumericEmbeddedStat
        {
            numericStatType = RoundStackingDamagePerGoldTreatEffectSO.statPerStack.numericStatType,
            numericStatModificationType = RoundStackingDamagePerGoldTreatEffectSO.statPerStack.numericStatModificationType,
            value = RoundStackingDamagePerGoldTreatEffectSO.statPerStack.value * stacks
        };

        return stackedEmbeddedStat;
    }


    private void GameManager_OnStateChanged(object sender, GameManager.OnStateChangeEventArgs e)
    {
        if(e.newState == GameManager.State.Combat)
        {
            isStacking = true;
            return;
        }

        if(e.previousState == GameManager.State.Combat)
        {
            ResetStacks();
            isStacking = false;
            return;
        }
    }

    private void GoldCollection_OnAnyGoldCollected(object sender, GoldCollection.OnGoldEventArgs e)
    {
        if (!isCurrentlyActiveByInventoryObjects) return;
        if (!isMeetingCondition) return;
        if (!isStacking) return;
        AddStacks(1);
    }
}
