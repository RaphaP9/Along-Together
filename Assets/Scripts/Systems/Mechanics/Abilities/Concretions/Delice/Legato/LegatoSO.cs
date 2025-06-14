using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegatoSO", menuName = "ScriptableObjects/Abilities/Delice/Legato")]
public class LegatoSO : ActiveAbilitySO
{
    [Header("Specific Settings")]
    [Range(1f, 5f)] public float flyDuration;
    public PhysicPushData pushData;
}
