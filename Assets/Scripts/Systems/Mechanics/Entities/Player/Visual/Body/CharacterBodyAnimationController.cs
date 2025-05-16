using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodyAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private EntityFacingDirectionHandler facingDirectionHandler;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMovement playerMovement;

    protected const string SPEED_FLOAT = "Speed";
    protected const string FACE_X_FLOAT = "FaceX";
    protected const string FACE_Y_FLOAT = "FaceY";

    protected const string MOVEMENT_BLEND_TREE_NAME = "MovementBlendTree";
    protected const string DEATH_ANIMATION_NAME = "Death";

    protected virtual void OnEnable()
    {
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
    }

    protected virtual void OnDisable()
    {
        playerHealth.OnPlayerDeath -= PlayerHealth_OnPlayerDeath;
    }

    protected virtual void Update()
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

    protected void PlayAnimation(string animationName) => animator.Play(animationName);

    #region Subscriptions
    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        PlayAnimation(DEATH_ANIMATION_NAME);
    }
    #endregion
}
