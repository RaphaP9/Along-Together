using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterBodyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private PlayerFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;
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
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;

        basicDash.OnPlayerDash += BasicDash_OnPlayerDash;
        basicDash.OnPlayerDashCompleted += BasicDash_OnPlayerDashCompleted;
    }

    protected virtual void OnDisable()
    {
        playerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;

        basicDash.OnPlayerDash -= BasicDash_OnPlayerDash;
        basicDash.OnPlayerDashCompleted -= BasicDash_OnPlayerDashCompleted;
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

    private void PlayAnimation(string animationName) => animator.Play(animationName);

    #region Subscriptions
    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        PlayAnimation(DEATH_ANIMATION_NAME);
    }

    private void BasicDash_OnPlayerDash(object sender, BasicDash.OnPlayerDashEventArgs e)
    {
        PlayAnimation(DASH_BLEND_TREE_NAME);
    }

    private void BasicDash_OnPlayerDashCompleted(object sender, BasicDash.OnPlayerDashEventArgs e)
    {
        PlayAnimation(MOVEMENT_BLEND_TREE_NAME);
    }
    #endregion
}
