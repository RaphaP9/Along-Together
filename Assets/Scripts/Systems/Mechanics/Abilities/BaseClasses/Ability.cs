using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilitySO abilitySO;
    [SerializeField] private AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilitySO;
    private AbilityLevel AbilityLevel => abilityLevel;

    private bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;
}
