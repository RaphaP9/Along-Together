using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonologueUI;
using static System.TimeZoneInfo;

public class VideoCinematicUI : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<VideoCinematicTransitionTypeAnimator> transitionTypeAnimators;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private const string HIDDEN_ANIMATION_NAME = "Hidden";
    private const string IDLE_ANIMATION_NAME = "Idle";

    private const string TRANSITION_IN_ANIMATION_NAME = "TransitionIn";
    private const string TRANSITION_OUT_ANIMATION_NAME = "TransitionOut";

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInEnd;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutEnd;

    private VideoCinematicSO currentVideoCinematic;

    public class OnVideoCinematicUIEventArgs : EventArgs
    {
        public VideoCinematicSO videoCinematicSO;
    }

    [Serializable]
    public class VideoCinematicTransitionTypeAnimator
    {
        public VideoCinematicTransitionType transitionType;
        public Animator animator;
    }


    private void SetCurrentVideoCinematic(VideoCinematicSO videoCinematicSO) => currentVideoCinematic = videoCinematicSO;

    #region Animations
    private void CinematicTransitionIn(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_IN_ANIMATION_NAME); 
        OnTransitionInStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionOut(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_OUT_ANIMATION_NAME);
        OnTransitionOutStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicIdle(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(IDLE_ANIMATION_NAME);
    }

    private void CinematicConcluded(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(HIDDEN_ANIMATION_NAME);
    }
    #endregion
    public void TransitionInEnd() => OnTransitionInEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionOutEnd() => OnTransitionOutEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });

    private Animator FindAnimatorByTransitionType(VideoCinematicTransitionType transitionType)
    {
        foreach (VideoCinematicTransitionTypeAnimator transitionTypeAnimator in transitionTypeAnimators)
        {
            if (transitionTypeAnimator.transitionType == transitionType) return transitionTypeAnimator.animator;
        }

        if (debug) Debug.Log($"Could not find animator for TransitionType: {transitionType}. Returning null Animator.");
        return null;

    }

}
