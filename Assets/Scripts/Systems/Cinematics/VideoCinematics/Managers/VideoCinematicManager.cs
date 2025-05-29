using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoCinematicManager : MonoBehaviour
{
    public static VideoCinematicManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private VideoCinematicSO currentVideoCinematicSO;

    [Header("States - Runtime Filled")]
    [SerializeField] private VideoCinematicState videoCinematicState;

    public VideoCinematicState State => videoCinematicState;

    public enum VideoCinematicState { NotOnCinematic, TransitionIn, Idle, TransitionOut }

    #region Flags
    private bool cinematicTransitionInOpeningCompleted = false;
    private bool cinematicTransitionInClosingCompleted = false;

    private bool cinematicTransitionOutOpeningCompleted = false;
    private bool cinematicTransitionOutClosingCompleted = false;

    private bool shouldSkipCinematic = false;
    #endregion

    #region Events
    public static event EventHandler<OnVideoCinematicEventArgs> OnCinematicBegin;
    public static event EventHandler<OnVideoCinematicEventArgs> OnCinematicEnd;

    public static event EventHandler<OnVideoCinematicEventArgs> OnCinematicIdle;

    public static event EventHandler<OnVideoCinematicEventArgs> OnCinematicPlayStart;
    public static event EventHandler<OnVideoCinematicEventArgs> OnCinematicPlayEnd;

    public static event EventHandler OnGeneralCinematicBegin;
    public static event EventHandler OnGeneralCinematicConcluded;
    #endregion

    public class OnVideoCinematicEventArgs : EventArgs
    {
        public VideoCinematicSO videoCinematicSO;
    }

    private void OnEnable()
    {
        VideoCinematicUI.OnTransitionInOpeningEnd += VideoCinematicUI_OnTransitionInOpeningEnd;
        VideoCinematicUI.OnTransitionInClosingEnd += VideoCinematicUI_OnTransitionInClosingEnd;

        VideoCinematicUI.OnTransitionOutOpeningEnd += VideoCinematicUI_OnTransitionOutOpeningEnd;
        VideoCinematicUI.OnTransitionOutClosingEnd += VideoCinematicUI_OnTransitionOutClosingEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one VideoCinematicManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void StartCinematic(VideoCinematicSO videoCinematicSO)
    {
        if (!CanStartCinematic()) return;

        //StartCoroutine(CinematicCoroutine(videoCinematicSO));
    }

    public void EndCinematic()
    {
        if (videoCinematicState != VideoCinematicState.Idle) return;
        shouldSkipCinematic = true;
    }

    /*
    private IEnumerator CinematicCoroutine(VideoCinematicSO videoCinematicSO)
    {
        OnGeneralCinematicBegin?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = videoCinematicSO});

        SetCurrentCinematic(videoCinematicSO);

        SetCinematicState(VideoCinematicState.TransitionIn);

        #region TransitionInOpeningLogic

        cinematicTransitionInOpeningCompleted = false;
        OnCinematicBegin?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = currentVideoCinematicSO });

        yield return new WaitUntil(() => cinematicTransitionInOpeningCompleted);
        cinematicTransitionInOpeningCompleted = false;
        #endregion

        SetCinematicState(VideoCinematicState.Idle);

        #region Idle Logic
        shouldSkipCinematic = false;
        OnCinematicIdle?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = currentVideoCinematicSO });

        float calculatedDuration = currentVideoCinematicSO.videoClip.frameCount / (float)currentVideoCinematicSO.videoClip.frameRate;

        #region Wait Sentence Time Logic

        float elapsedTime = 0;

        while (elapsedTime <= calculatedDuration)
        {
            if (shouldSkipCinematic) break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        #endregion

        shouldSkipCinematic = false;

        OnCinematicEnd?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = currentVideoCinematicSO });

        #region Transition Out Logic
        SetCinematicState(VideoCinematicState.TransitionIn);

        cinematicTransitionOutCompleted = false;
        OnCinematicBegin?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = currentVideoCinematicSO });

        yield return new WaitUntil(() => cinematicTransitionOutCompleted);
        cinematicTransitionOutCompleted = false;
        #endregion

        OnGeneralCinematicConcluded?.Invoke(this, new OnVideoCinematicEventArgs { videoCinematicSO = currentVideoCinematicSO });
        SetCinematicState(VideoCinematicState.NotOnCinematic);
        ClearCurrentCinematic(); 
    }
    #endregion
    */

    private bool CanStartCinematic()
    {
        if (videoCinematicState != VideoCinematicState.NotOnCinematic) return false;
        return true;
    }

    #region States
    private void SetCinematicState(VideoCinematicState videoCinematicState) => this.videoCinematicState = videoCinematicState;

    #endregion

    #region Setters
    private void SetCurrentCinematic(VideoCinematicSO videoCinematicSO) => currentVideoCinematicSO = videoCinematicSO;
    private void ClearCurrentCinematic() => currentVideoCinematicSO = null;
    #endregion

    #region Subscriptions
    private void VideoCinematicUI_OnTransitionInOpeningEnd(object sender, VideoCinematicUI.OnVideoCinematicUIEventArgs e) => cinematicTransitionInOpeningCompleted = true;
    private void VideoCinematicUI_OnTransitionInClosingEnd(object sender, VideoCinematicUI.OnVideoCinematicUIEventArgs e) => cinematicTransitionInClosingCompleted = true;

    private void VideoCinematicUI_OnTransitionOutOpeningEnd(object sender, VideoCinematicUI.OnVideoCinematicUIEventArgs e) => cinematicTransitionOutOpeningCompleted = true;
    private void VideoCinematicUI_OnTransitionOutClosingEnd(object sender, VideoCinematicUI.OnVideoCinematicUIEventArgs e) => cinematicTransitionOutClosingCompleted = true;
    #endregion
}
