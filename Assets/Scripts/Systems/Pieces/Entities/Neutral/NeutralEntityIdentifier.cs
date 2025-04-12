using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEntityIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NeutralEntitySO neutralEntitySO;

    public NeutralEntitySO NeutralEntitySO => neutralEntitySO;
}
