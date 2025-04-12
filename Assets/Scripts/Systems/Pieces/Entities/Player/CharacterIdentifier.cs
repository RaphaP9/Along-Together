using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdentifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterSO characterSO;

    public CharacterSO CharacterSO => characterSO;
}

