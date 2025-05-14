using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AllySO allySO;

    public AllySO AllySO => allySO;
}
