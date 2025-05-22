using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class PlayerNumericStatUI<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected TextMeshProUGUI valueText;

    [Header("Settings")]
    [SerializeField] protected Color positiveColor;
    [SerializeField] protected Color neutralColor;
    [SerializeField] protected Color negativeColor;
    [Space]
    [SerializeField] protected bool invertEvaluation; 
    //A greater value than base value is considered Positive by default
    //If invertEvaluation checked, it is considered Negative

    [Header("Debug")]
    [SerializeField] protected bool debug;

    protected T resolver;

    protected enum StatState { Positive, Neutral, Negative}

    protected virtual void OnEnable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation += PlayerInstantiationHandler_OnPlayerInstantiation;
    }
    protected virtual void OnDisable()
    {
        PlayerInstantiationHandler.OnPlayerInstantiation -= PlayerInstantiationHandler_OnPlayerInstantiation;
        UnSubscribeToEvents();
    }

    #region Logic
    protected void UpdateUIByNewValue(float currentValue, float baseValue)
    {
        StatState statState = GetStatState(currentValue, baseValue);

        switch (statState)
        {
            case StatState.Positive:
                SetValueTextColor(positiveColor);
                break;
            case StatState.Neutral:
                SetValueTextColor(neutralColor);
                break;
            case StatState.Negative:
                SetValueTextColor(negativeColor);
                break;
        }

        string processedValueText = ProcessCurrentValue(currentValue);
        SetValueText(processedValueText);
    }

    protected void SetValueText(string text) => valueText.text = text;
    protected void SetValueTextColor(Color color) => valueText.color = color;

    protected StatState GetStatState(float currentValue, float baseValue)
    {
        if(currentValue > baseValue)
        {
            if(!invertEvaluation) return StatState.Positive;
            return StatState.Negative;
        }

        if(currentValue < baseValue)
        {
            if(!invertEvaluation) return StatState.Negative;
            return StatState.Positive;
        }

        return StatState.Neutral;
    }

    protected abstract string ProcessCurrentValue(float currentValue);
    protected abstract float GetCurrentValue();
    protected abstract float GetBaseValue();
    #endregion

    protected void FindResolverLogic(Transform playerTransform)
    {
        resolver = playerTransform.GetComponentInChildren<T>();

        if (resolver == null)
        {
            if (debug) Debug.Log("Could not find resolver.");
            return;
        }

        SubscribeToEvents();
    }

    protected abstract void SubscribeToEvents();
    protected abstract void UnSubscribeToEvents();

    #region Subscriptions
    private void PlayerInstantiationHandler_OnPlayerInstantiation(object sender, PlayerInstantiationHandler.OnPlayerInstantiationEventArgs e)
    {
        FindResolverLogic(e.playerTransform);
    }
    #endregion
}
