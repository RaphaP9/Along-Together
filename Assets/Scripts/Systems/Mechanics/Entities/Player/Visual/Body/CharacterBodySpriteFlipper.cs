using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodySpriteFlipper : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerAimDirectionerHandler aimDirectionerHandler;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Space]
    [SerializeField] private CharacterFacingHandler characterFacingHandler;

    private bool facingRight = true;

    private void Update()
    {
        HandleFacingDueToAim();
    }

    private void HandleFacingDueToAim()
    {
        if (!characterFacingHandler.CanFace()) return;

        if (aimDirectionerHandler.IsAimingRight())
        {
            CheckFlipRight();
        }

        if (!aimDirectionerHandler.IsAimingRight())
        {
            CheckFlipLeft();
        }
    }

    private void CheckFlipRight()
    {
        if (facingRight) return;

        FlipRight();

        facingRight = true;
    }

    private void CheckFlipLeft()
    {
        if (!facingRight) return;

        FlipLeft();

        facingRight = false;
    }

    private void FlipRight()
    {
        spriteRenderer.flipX = false;
    }

    private void FlipLeft()
    {
        spriteRenderer.flipX = true;
    }
}
