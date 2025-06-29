using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StackingTreatEffectHandler : TreatEffectHandler
{
    [Header("Stacking Runtime Filled")]
    [SerializeField] protected int stacks;

    protected bool isStacking = false;

    public static event EventHandler<OnStackEventArgs> OnStacksGained;
    public static event EventHandler<OnStackEventArgs> OnStacksLost;
    public static event EventHandler<OnStackEventArgs> OnStacksReset;

    public class OnStackEventArgs : EventArgs
    {
        public int stacks;
    }

    protected virtual void AddStacks(int quantity)
    {
        stacks += quantity;
        OnStacksGained?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }

    protected virtual void RemoveStacks(int quantity)
    {
        stacks = stacks - quantity <0? 0: stacks-quantity;
        OnStacksLost?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }

    protected virtual void ResetStacks()
    {
        stacks = 0;
        OnStacksReset?.Invoke(this, new OnStackEventArgs { stacks = stacks });
    }
}
