using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Runtime Filled")]
    [SerializeField] private float timer = 0f;

    #region Properties
    public float StartingGameTimer => startingGameTimer;
    public float RoundStartingTime => roundStartingTime;
    public float RoundEndingTime => roundEndingTime;
    public float ChangeStateStartingTimer => changeStateStartingTimer;
    public float ChangeStateEndingTimer => changeStateEndingTimer;
    #endregion

    //Monologue is considered non GameState intrusive, can happen on combat,etc
    public enum State {StartingGame, BeginningCombat, Combat, EndingCombat, Shop, Upgrade, BeginningChangingStage, EndingChangingStage, Cinematic, Dialogue, Lose, Win } 

    public State GameState => state;

    public static event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public static event EventHandler<OnStateInitializedEventArgs> OnStateInitialized;

    private bool firstUpdateLogicPerformed = false;
    private bool shouldChangeToNextStage = false;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State previousState;
        public State newState;
    }

    public class OnStateInitializedEventArgs : EventArgs
    {
        public State state;
    }

    private void OnEnable()
    {
        ShopOpeningManager.OnShopClose += ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately += ShopOpeningManager_OnShopCloseImmediately;

        GeneralStagesManager.OnRoundEnd += GeneralStagesManager_OnRoundEnd;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately -= ShopOpeningManager_OnShopCloseImmediately;

        GeneralStagesManager.OnRoundEnd -= GeneralStagesManager_OnRoundEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetGameState(State.StartingGame);
        ResetTimer();
    }

    private void Update()
    {
        FirstUpdateLogic();
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

    private void InitializeState(State state)
    {
        SetGameState(state);
        OnStateInitialized?.Invoke(this, new OnStateInitializedEventArgs {state = state});
    }
    #endregion

    #region Logic
    private void FirstUpdateLogic()
    {
        if (firstUpdateLogicPerformed) return;

        InitializeState(State.StartingGame);
        GeneralStagesManager.Instance.InitializeToCurrentStage();
        firstUpdateLogicPerformed = true;
    }

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
            case State.BeginningChangingStage:
                BeginningChangingStageLogic();
                break;
            case State.EndingChangingStage:
                EndingChangingStageLogic();
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
            case State.Win:
                WinLogic();
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

        if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage())
        {
            ChangeState(State.Shop); //Later Change to Upgrade
            ShopOpeningManager.Instance.OpenShop();
        }
        else
        {
            ChangeState(State.Shop);
            ShopOpeningManager.Instance.OpenShop();
        }

        ResetTimer();
    }

    private void BeginningCombatLogic()
    {
        if (timer < roundStartingTime)
        {
            timer += Time.deltaTime;
            return;
        }

        ChangeState(State.Combat);
        ResetTimer();

        GeneralStagesManager.Instance.StartCurrentRound();
    }

    private void CombatLogic()
    {
        ResetTimer();
    }

    private void EndingCombatLogic()
    {
        if (timer < roundEndingTime)
        {
            timer += Time.deltaTime;
            return;
        }

        if (GeneralStagesManager.Instance.CurrentStageAndRoundNumberAreLasts())
        {
            ChangeState(State.Win);
            ResetTimer();
            return;
        }

        if (GeneralStagesManager.Instance.CurrentRoundIsLastFromCurrentStage())
        {
            ChangeState(State.BeginningChangingStage);
        }
        else
        {
            ChangeState(State.Shop);
            ShopOpeningManager.Instance.OpenShop();
        }

        ResetTimer();
    }

    private void ShopLogic()
    {
        ResetTimer();
    }

    private void UpgradeLogic()
    {
        ResetTimer();
    }

    private void BeginningChangingStageLogic()
    {
        if (timer < changeStateStartingTimer)
        {
            timer += Time.deltaTime;
            return;
        }

        GeneralStagesManager.Instance.ChangeToCurrentStage();
        ResetTimer();
    }

    private void EndingChangingStageLogic()
    {
        if (timer < changeStateEndingTimer)
        {
            timer += Time.deltaTime;
            return;
        }

        ShopOpeningManager.Instance.OpenShop(); //Later Change To Upgrade
        ResetTimer();
    }

    private void CinematicLogic()
    {
        ResetTimer();
    }

    private void DialogueLogic()
    {
        ResetTimer();
    }

    private void LoseLogic()
    {
        ResetTimer();
    }

    private void WinLogic()
    {
        ResetTimer();
    }
    #endregion

    private void ResetTimer() => timer = 0f;

    #region Subscriptions
    private void ShopOpeningManager_OnShopClose(object sender, EventArgs e)
    {
        ChangeState(State.BeginningCombat);
    }

    private void ShopOpeningManager_OnShopCloseImmediately(object sender, EventArgs e)
    {
        ChangeState(State.BeginningCombat);
    }

    private void GeneralStagesManager_OnRoundEnd(object sender, GeneralStagesManager.OnRoundEventArgs e)
    {
        ChangeState(State.EndingCombat);
    }

    #endregion
}
