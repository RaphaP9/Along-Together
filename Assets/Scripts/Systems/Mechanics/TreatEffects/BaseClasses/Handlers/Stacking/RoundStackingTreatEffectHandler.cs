using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundStackingTreatEffectHandler : StackingTreatEffectHandler
{
    protected virtual void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
    }

    protected abstract string GetRefferencialGUID();

    protected override void AddStacks(int quantity)
    {
        base.AddStacks(quantity);
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //Remove Stats from previous stacked value, in inherited classes, we Add the stat modifiers
    }

    protected override void ResetStacks()
    {
        base.ResetStacks();
        TemporalNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID());
    }

    #region Subscriptions
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
    #endregion
}
