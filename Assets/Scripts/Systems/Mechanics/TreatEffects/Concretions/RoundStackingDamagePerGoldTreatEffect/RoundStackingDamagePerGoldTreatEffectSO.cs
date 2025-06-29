using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundStackingDamagePerGoldTreatEffectSO", menuName = "ScriptableObjects/TreatEffects/RoundStackingDamagePerGoldTreatEffect")]
public class RoundStackingDamagePerGoldTreatEffectSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}

