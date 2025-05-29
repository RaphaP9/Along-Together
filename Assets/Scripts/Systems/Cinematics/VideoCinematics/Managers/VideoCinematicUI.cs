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

    private const string TRANSITION_IN_OPENING_ANIMATION_NAME = "TransitionInOpening";
    private const string TRANSITION_IN_CLOSING_ANIMATION_NAME = "TransitionInClosing";

    private const string TRANSITION_OUT_OPENING_ANIMATION_NAME = "TransitionOutOpening";
    private const string TRANSITION_OUT_CLOSING_ANIMATION_NAME = "TransitionOutClosing";

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInOpeningStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInOpeningEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInClosingStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionInClosingEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutOpeningStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutOpeningEnd;

    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutClosingStart;
    public static event EventHandler<OnVideoCinematicUIEventArgs> OnTransitionOutClosingEnd;

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

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void SetCurrentVideoCinematic(VideoCinematicSO videoCinematicSO) => currentVideoCinematic = videoCinematicSO;

    #region Animations
    private void CinematicTransitionInOpening(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_IN_OPENING_ANIMATION_NAME); 
        OnTransitionInOpeningStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionInClosing(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_IN_CLOSING_ANIMATION_NAME);
        OnTransitionInClosingStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionOutOpening(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_OUT_OPENING_ANIMATION_NAME);
        OnTransitionOutOpeningStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
    }

    private void CinematicTransitionOutClosing(VideoCinematicSO videoCinematicSO)
    {
        Animator transitionAnimator = FindAnimatorByTransitionType(videoCinematicSO.transitionType);

        if (transitionAnimator == null) return;

        transitionAnimator.Play(TRANSITION_OUT_CLOSING_ANIMATION_NAME);
        OnTransitionOutClosingStart?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = videoCinematicSO });
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
    public void TransitionInOpeningEnd() => OnTransitionInOpeningEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionInClosingEnd() => OnTransitionInClosingEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionOutOpeningEnd() => OnTransitionOutOpeningEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });
    public void TransitionOutClosingEnd() => OnTransitionOutClosingEnd?.Invoke(this, new OnVideoCinematicUIEventArgs { videoCinematicSO = currentVideoCinematic });

    private Animator FindAnimatorByTransitionType(VideoCinematicTransitionType transitionType)
    {
        foreach (VideoCinematicTransitionTypeAnimator transitionTypeAnimator in transitionTypeAnimators)
        {
            if (transitionTypeAnimator.transitionType == transitionType) return transitionTypeAnimator.animator;
        }

        if (debug) Debug.Log($"Could not find animator for TransitionType: {transitionType}. Returning null Animator.");
        return null;

    }

    #region Subscriptions

    #endregion

}
