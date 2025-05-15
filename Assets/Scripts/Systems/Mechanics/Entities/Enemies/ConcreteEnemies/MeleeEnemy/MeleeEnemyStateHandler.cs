using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyStateHandler : MonoBehaviour
{
    [Header("State - Runtime Filled")]
    [SerializeField] private MeleeEnemyState meleeEnemyState;

    private enum MeleeEnemyState { Spawning, }
}
