using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySlowStatusEffectHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float slowResistance;

    [Header("Runtime Filled - Lists")]
    [SerializeField] private List<SlowStatusEffect> slowStatusEffects;

    [Header("Runtime Filled")]
    [SerializeField] private float slowPercentageResolvedValue;

    public List<SlowStatusEffect> SlowStatusEffects => slowStatusEffects;
    public float SlowPercentageResolvedValue => slowPercentageResolvedValue;

    private const float MIN_SLOW_PERCENTAGE_VALUE = 0f;
    private const float MAX_SLOW_PERCENTAGE_VALUE = 0.9f;

    public event EventHandler<OnSlowStatusEffectValueEventArgs> OnSlowStatusEffectValueRecauculated;

    public class OnSlowStatusEffectValueEventArgs : EventArgs
    {
        public float slowValue;
    }

    public void SlowEntity(SlowStatusEffect slowStatusEffect)
    {
        StartCoroutine(AddSlowStatusEffectForDurationTime(slowStatusEffect));
    }

    private IEnumerator AddSlowStatusEffectForDurationTime(SlowStatusEffect slowStatusEffect)
    {
        slowStatusEffects.Add(slowStatusEffect);
        RecalculateSlowPercentageResolvedValue();

        yield return new WaitForSeconds(slowStatusEffect.duration);

        slowStatusEffects.Remove(slowStatusEffect);
        RecalculateSlowPercentageResolvedValue();
    }

    private void RecalculateSlowPercentageResolvedValue()
    {
        if(slowStatusEffects.Count <= 0)
        {
            slowPercentageResolvedValue = MIN_SLOW_PERCENTAGE_VALUE;
            OnSlowStatusEffectValueRecauculated?.Invoke(this, new OnSlowStatusEffectValueEventArgs { slowValue = slowPercentageResolvedValue });
            return;
        }

        float highestSlowPercentageValue = 0f;

        foreach(SlowStatusEffect slowStatusEffect in slowStatusEffects)
        {
            if(slowStatusEffect.slowPercentage >  highestSlowPercentageValue) highestSlowPercentageValue = slowStatusEffect.slowPercentage;
        }

        slowPercentageResolvedValue = (highestSlowPercentageValue > MAX_SLOW_PERCENTAGE_VALUE? MAX_SLOW_PERCENTAGE_VALUE: highestSlowPercentageValue) * (1-slowResistance);
        OnSlowStatusEffectValueRecauculated?.Invoke(this, new OnSlowStatusEffectValueEventArgs { slowValue = slowPercentageResolvedValue });
    }


}