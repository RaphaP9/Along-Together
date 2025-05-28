using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonologueManager;

public class VideoCinematicManager : MonoBehaviour
{
    public static VideoCinematicManager Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private VideoCinematicSO currentVideoCinematicSO;

    [Header("States - Runtime Filled")]
    [SerializeField] private VideoCinematicState videoCinematicState;

    public VideoCinematicState State => videoCinematicState;

    public enum VideoCinematicState { NotOnCinematic, TransitionIn, Playing, TransitionOut }

    #region Flags
    private bool cinematicTransitionInCompleted = false;
    private bool cinematicTransitionOutCompleted = false;
    #endregion

    #region Events
    public static event EventHandler<OnMonologueEventArgs> OnCinematicBegin;
    public static event EventHandler<OnMonologueEventArgs> OnCinematicEnd;

    public static event EventHandler<OnMonologueEventArgs> OnCinematicIdle;

    public static event EventHandler OnGeneralCinematicBegin;
    public static event EventHandler OnGeneralCinematicConcluded;
    #endregion

    public class OnVideoCinematicEventArgs : EventArgs
    {
        public VideoCinematicSO videoCinematicSO;
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


    private bool CanStartMonologue()
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
}
