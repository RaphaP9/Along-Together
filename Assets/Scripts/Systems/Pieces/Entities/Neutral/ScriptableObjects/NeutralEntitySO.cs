using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNeutralEntitySO", menuName = "ScriptableObjects/Entities/NeutralEntity")]
public class NeutralEntitySO : EntitySO
{
    [Header("Neutral Entity Settings")]
    public Transform neutralEntityPrefab;
}
