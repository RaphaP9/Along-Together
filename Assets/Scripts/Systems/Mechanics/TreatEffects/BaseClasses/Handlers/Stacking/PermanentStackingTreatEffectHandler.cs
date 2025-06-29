using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PermanentStackingTreatEffectHandler : StackingTreatEffectHandler
{
    private void Start()
    {
        RecoverSavedStacks();
    }

    protected abstract NumericEmbeddedStat GetRefferencialNumericEmbeddedStatPerStack();
    protected abstract string GetRefferencialGUID();

    //The saved refferencial numeric stat type must coincide with the refferencial numeric embedded stat per stack
    protected NumericStatModifier FindSavedRefferencialNumericStatModifier() => RunNumericStatModifierManager.Instance.GetFirstNumericStatModifierByGUIDAndNumericStatType(GetRefferencialGUID(), GetRefferencialNumericEmbeddedStatPerStack().numericStatType);

    private void RecoverSavedStacks()
    {
        NumericStatModifier savedRefferencialNumericStatModifier = FindSavedRefferencialNumericStatModifier();

        if(savedRefferencialNumericStatModifier == null) //If there is no saved numeric stat modifier
        {
            ResetStacks();
            return;
        }

        //Use a proportional relation to recover saved stacks. Ex. if saved stacks value is 6 and we have a referencial numeric embedded stack with value 0.5, we can conclude we have 12 stacks saved
        int savedStacks = Mathf.RoundToInt(savedRefferencialNumericStatModifier.value / GetRefferencialNumericEmbeddedStatPerStack().value); 
        SetStacks(savedStacks);
    }

    protected override void OnTreatDeactivatedByInventoryObjectsMethod() 
    {
        base.OnTreatDeactivatedByInventoryObjectsMethod();
        RunNumericStatModifierManager.Instance.RemoveStatModifiersByGUID(GetRefferencialGUID()); //Remove Stacks from Run Numeric Stat Modifier List
    }
}
