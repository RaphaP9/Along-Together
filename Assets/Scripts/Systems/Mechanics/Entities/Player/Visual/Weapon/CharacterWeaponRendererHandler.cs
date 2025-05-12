using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponRendererHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Transform> attackInterruptionAbilitiesTransforms;

    private List<IAttackInterruptionAbility> attackInterruptionAbilities;
    private bool renderingWeapon = true;

    private void Awake()
    {
        GetAttackInterruptionAbilitiesInterfaces();
    }

    private void Update()
    {
        HandleWeaponRendering();
    }

    private void GetAttackInterruptionAbilitiesInterfaces()
    {
        attackInterruptionAbilities = GeneralUtilities.TryGetGenericsFromTransforms<IAttackInterruptionAbility>(attackInterruptionAbilitiesTransforms);
    }

    private void HandleWeaponRendering()
    {
        if (CanRenderWeapon())
        {
            CheckShouldRender();
        }

        if (!CanRenderWeapon())
        {
            CheckStopRender();
        }
    }

    private void CheckShouldRender()
    {
        if (renderingWeapon) return;

        spriteRenderer.enabled = true;

        renderingWeapon = true;
    }

    private void CheckStopRender()
    {
        if (!renderingWeapon) return;

        spriteRenderer.enabled = false;

        renderingWeapon = false;
    }


    protected virtual bool CanRenderWeapon()
    {
        foreach (IAttackInterruptionAbility attackInterruptionAbility in attackInterruptionAbilities)
        {
            if (attackInterruptionAbility.IsInterruptingAttack()) return false;
        }

        return true;
    }

}
