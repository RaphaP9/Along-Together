using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StaccatoSO", menuName = "ScriptableObjects/Abilities/Delice/Staccato")]
public class StaccatoSO : ActiveAbilitySO, IHasEmbeddedNumericStats
{
    [Header("Specific Settings")]
    public List<NumericEmbeddedStat> numericEmbeddedStats;
    [Space]
    [Range(0.5f, 5f)] public float duration;
    [Range(0f, 1f)] public float performanceTime;
    [Range(0.05f, 0.2f)] public float burstInterval;

    public List<NumericEmbeddedStat> GetNumericEmbeddedStats() => numericEmbeddedStats;
}
