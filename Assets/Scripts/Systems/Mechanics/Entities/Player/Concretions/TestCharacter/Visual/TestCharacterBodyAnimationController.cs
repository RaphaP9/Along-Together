using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterBodyAnimationController : CharacterBodyAnimationController
{
    [Header("Specific Components")]
    [SerializeField] private BasicDash basicDash;

    private const string DASH_BLEND_TREE_NAME = "DashBlendTree";

    protected override void OnEnable()
    {
        basicDash.OnPlayerDash += BasicDash_OnPlayerDash;
        basicDash.OnPlayerDashCompleted += BasicDash_OnPlayerDashCompleted;
    }

    protected override void OnDisable()
    {
        basicDash.OnPlayerDash -= BasicDash_OnPlayerDash;
        basicDash.OnPlayerDashCompleted -= BasicDash_OnPlayerDashCompleted;
    }

    #region Subscriptions
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
