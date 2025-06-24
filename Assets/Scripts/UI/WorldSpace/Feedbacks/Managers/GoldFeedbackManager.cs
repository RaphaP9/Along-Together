using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFeedbackManager : NumericFeedbackManager
{
    [Header("Specific Settings")]
    [SerializeField, ColorUsage(true, true)] private Color feedbackColor;

    private void OnEnable()
    {
        GoldManager.OnTangibleGoldCollected += GoldManager_OnTangibleGoldCollected;
    }
    private void OnDisable()
    {
        GoldManager.OnTangibleGoldCollected -= GoldManager_OnTangibleGoldCollected;
    }

    private void GoldManager_OnTangibleGoldCollected(object sender, GoldManager.OnTangibleGoldEventArgs e)
    {
        Vector2 instantiationPosition = GetInstantiationPosition(e.position);
        CreateNumericFeedback(feedbackPrefab, instantiationPosition, e.goldAmount, feedbackColor);
    }
}
