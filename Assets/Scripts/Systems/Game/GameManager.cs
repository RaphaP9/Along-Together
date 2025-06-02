using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    //Monologue is considered non GameState intrusive, can happen on combat,etc
    public enum State {StartingGame, BeginningCombat, Combat, EndingCombat, Shop, Upgrade, ChangingStage, Cinematic, Dialogue, Lose } 

    public State GameState => state;

    public static event EventHandler<OnStateChangeEventArgs> OnStateChanged;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State previousState;
        public State newState;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        SetSingleton();
    }

    #region Initialization
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    #region Utility Methods
    private void SetGameState(State state)
    {
        SetPreviousState(this.state);
        this.state = state;
    }

    private void SetPreviousState(State state)
    {
        previousState = state;
    }

    private void ChangeState(State state)
    {
        State previousState = this.state;
        SetGameState(state);
        OnStateChanged?.Invoke(this, new OnStateChangeEventArgs {previousState = previousState,  newState = state });
    }
    #endregion

}
