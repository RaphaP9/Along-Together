using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFacingHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<Transform> facingInterruptionAbilitiesTransforms;

    private List<IFacingInterruptionAbility> facingInterruptionAbilities;

    private void Awake()
    {
        GetFacingInterruptionAbilitiesInterfaces();
    }

    private void GetFacingInterruptionAbilitiesInterfaces()
    {
        facingInterruptionAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IFacingInterruptionAbility>(facingInterruptionAbilitiesTransforms);
    }

    public bool CanFace()
    {
        foreach (IFacingInterruptionAbility facingInterruptionAbility in facingInterruptionAbilities)
        {
            if (facingInterruptionAbility.IsInterruptingFacing()) return false;
        }

        return true;
    }
}
