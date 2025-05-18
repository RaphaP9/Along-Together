using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLevelHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AbilityIdentifier abilityIdentifier;

    [Header("Runtime Filled")]
    [SerializeField] private AbilityLevel abilityLevel;

    public AbilitySO AbilitySO => abilityIdentifier.AbilitySO;
    public AbilityLevel AbilityLevel => abilityLevel;

    public event EventHandler<OnAbilityLevelEventArgs> OnAbilityLevelSet;
    public static event EventHandler<OnAbilityLevelEventArgs> OnAnyAbilityLevelSet;

    public event EventHandler<OnAbilityLevelEventArgs> OnAbilityLevelInitialized;
    public static event EventHandler<OnAbilityLevelEventArgs> OnAnyAbilityLevelInitialized;

    public class OnAbilityLevelEventArgs : EventArgs
    {
        public AbilitySO abilitySO;
        public AbilityLevel abilityLevel;
    }

    public bool IsUnlocked() => abilityLevel != AbilityLevel.NotLearned;

    public void SetAbilityLevel(AbilityLevel abilityLevel)
    {
        this.abilityLevel = abilityLevel;

        OnAbilityLevelSet?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel });
        OnAnyAbilityLevelSet?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel });
    }

    public void InitializeAbilityLevel(AbilityLevel abilityLevel)
    {
        this.abilityLevel = abilityLevel;

        OnAbilityLevelInitialized?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel });
        OnAnyAbilityLevelInitialized?.Invoke(this, new OnAbilityLevelEventArgs { abilitySO = AbilitySO, abilityLevel = abilityLevel });
    }
}
