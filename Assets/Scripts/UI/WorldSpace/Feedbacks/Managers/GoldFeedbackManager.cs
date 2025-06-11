using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFeedbackManager : NumericFeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        GoldDropperManager.OnEntityDropGold += GoldDropperManager_OnEntityDropGold;
    }

    private void OnDisable()
    {
        GoldDropperManager.OnEntityDropGold -= GoldDropperManager_OnEntityDropGold;
    }

    private void GoldDropperManager_OnEntityDropGold(object sender, GoldDropperManager.OnEntityDropGoldEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.entityPosition);
        CreateFeedback(numericUIPrefab, instantiationPosition, e.goldAmount, feedbackColor);
    }
}
