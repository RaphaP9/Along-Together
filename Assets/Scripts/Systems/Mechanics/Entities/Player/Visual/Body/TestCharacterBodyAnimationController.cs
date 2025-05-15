using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterBodyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [Space]
    [SerializeField] private BasicDash basicDash;

    private const string SPEED_FLOAT = "Speed";
    private const string FACE_X_FLOAT = "FaceX";
    private const string FACE_Y_FLOAT = "FaceY";

    private const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    private const string DASH_BLEND_TREE_NAME = "DashBlendTree";
    private const string DEATH_ANIMATION_NAME = "Death";

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    private void Update()
    {
        HandleSpeedBlend();
        HandleFacingBlend();
    }

    private void HandleSpeedBlend()
    {
        animator.SetFloat(SPEED_FLOAT, playerMovement.DesiredSpeed);
    }

    private void HandleFacingBlend()
    {
        animator.SetFloat(FACE_X_FLOAT, facingDirectionHandler.CurrentFacingDirection.x);
        animator.SetFloat(FACE_Y_FLOAT, facingDirectionHandler.CurrentFacingDirection.y);
    }
}
