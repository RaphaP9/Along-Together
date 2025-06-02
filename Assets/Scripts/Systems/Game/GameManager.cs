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

    [Header("Settings - Timers")]
    [SerializeField, Range(2f, 5f)] private float startingGameTimer;
    [Space]
    [SerializeField, Range(2f, 5f)] private float roundStartingTime;
    [SerializeField, Range(2f, 5f)] private float roundEndingTime;
    [Space]
    [SerializeField, Range(2f, 5f)] private float changeStateStartingTimer;
    [SerializeField, Range(2f, 5f)] private float changeStateEndingTimer;

    #region Properties
    public float StartingGameTimer => startingGameTimer;
    public float RoundStartingTime => roundStartingTime;
    public float RoundEndingTime => roundEndingTime;
    public float ChangeStateStartingTimer => changeStateStartingTimer;
    public float ChangeStateEndingTimer => changeStateEndingTimer;
    #endregion

    //Monologue is considered non GameState intrusive, can happen on combat,etc
    public enum State {StartingGame, BeginningCombat, Combat, EndingCombat, Shop, Upgrade, ChangingStage, Cinematic, Dialogue, Lose } 

    public State GameState => state;

    public static event EventHandler<OnStateChangeEventArgs> OnStateChanged;

    private float timer = 0f;

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

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        HandleGameStates();
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

    #region Logic
    private void HandleGameStates()
    {
        switch (state)
        {
            case State.StartingGame:
                StartingGameLogic();
                break;
            case State.BeginningCombat:
                BeginningCombatLogic();
                break;
            case State.Combat:
                CombatLogic();
                break;
            case State.EndingCombat:
                EndingCombatLogic();
                break;
            case State.Shop:
                ShopLogic();
                break;
            case State.Upgrade:
                UpgradeLogic();
                break;
            case State.ChangingStage:
                ChangingStageLogic();
                break;
            case State.Cinematic:
                CinematicLogic();
                break;
            case State.Dialogue:
                DialogueLogic();
                break;
            case State.Lose:
                LoseLogic();
                break;

        }
    }

    private void StartingGameLogic()
    {
        if(timer < startingGameTimer)
        {
            timer += Time.deltaTime;
            return;
        }

        ResetTimer();

        //if(GeneralStagesManager.Instance.cU)
    }

    private void BeginningCombatLogic()
    {

    }

    private void CombatLogic()
    {

    }

    private void EndingCombatLogic()
    {

    }

    private void ShopLogic()
    {

    }

    private void UpgradeLogic()
    {

    }

    private void ChangingStageLogic()
    {

    }

    private void CinematicLogic()
    {

    }

    private void DialogueLogic()
    {

    }

    private void LoseLogic()
    {

    }
    #endregion

    private void ResetTimer() => timer = 0f;

    #region Subscriptions

    #endregion
}
