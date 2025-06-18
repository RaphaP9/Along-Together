using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegatoSO", menuName = "ScriptableObjects/Abilities/Delice/Legato")]
public class LegatoSO : ActiveAbilitySO
{
    [Header("Specific Settings")]
    [Range(0f, 1f)] public float flyStartDuration;
    [Range(1f, 5f)] public float flyDuration;
    [Range(0f, 1f)] public float flyEndDuration;
    [Space]
    public PhysicPushData pushData;
}
