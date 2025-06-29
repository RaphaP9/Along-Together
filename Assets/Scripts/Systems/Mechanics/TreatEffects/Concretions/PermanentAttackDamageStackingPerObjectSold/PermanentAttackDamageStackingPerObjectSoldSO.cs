using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanentAttackDamageStackingPerObjectSoldSO", menuName = "ScriptableObjects/TreatEffects/PermanentAttackDamageStackingPerObjectSold")]
public class PermanentAttackDamageStackingPerObjectSoldSO : TreatEffectSO
{
    [Header("Specific Settings")]
    public string refferencialGUID;
    [Space]
    public NumericEmbeddedStat statPerStack;
}



