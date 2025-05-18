using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySO abilitySO;

    public AbilitySO AbilitySO => abilitySO;
}
