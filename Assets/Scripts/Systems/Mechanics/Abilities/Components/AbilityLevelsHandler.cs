using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLevelsHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<AbilityLevelGroup> abilityLevelGroups;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<AbilityLevelGroup> AbilityLevelGroups => abilityLevelGroups;


    [Serializable]
    public class AbilityLevelGroup
    {
        public Ability ability;

        public AbilityLevel startingAbilityLevel;

        [Header("Runtime Filled")]
        public AbilityLevel currentAbilityLevel;
    }
}
