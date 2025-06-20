using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritardando : ActiveAbility
{
    [Header("Specific Settings")]
    [SerializeField] private LayerMask effectLayerMask; //For both damage & slow

    private RitardandoSO RitardandoSO => AbilitySO as RitardandoSO;
}
