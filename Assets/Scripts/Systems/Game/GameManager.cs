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
    [Space]
    [SerializeField, Range(0f, 2f)] private float dialogueInterval;

    public static event EventHandler OnTriggerDataSave;

    //Monologue is considered non GameState intrusive, can happen on combat,etc
    public enum State {StartingGame, BeginningCombat, Combat, EndingCombat, Shop, Upgrade, BeginningChangingStage, EndingChangingStage, Cinematic, Dialogue, Lose, Win } 

    public State GameState => state;

    public static event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public static event EventHandler<OnStateInitializedEventArgs> OnStateInitialized;

    #region Flags
    private bool firstUpdateLogicPerformed = false;
    private bool dialogueConcluded = false;
    private bool shopClosed = false;
    private bool abilityUpgradeClosed = false;
    private bool roundEnded = false;
    //private bool cinematicConcluded = false;
    #endregion

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

        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose += AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately += AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;

        GeneralStagesManager.OnRoundEnd += GeneralStagesManager_OnRoundEnd;

        DialogueManager.OnGeneralDialogueConcluded += DialogueManager_OnGeneralDialogueConcluded;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately -= ShopOpeningManager_OnShopCloseImmediately;

        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose -= AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately -= AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;

        GeneralStagesManager.OnRoundEnd -= GeneralStagesManager_OnRoundEnd;

        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        FirstUpdateLogic();
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

        StartCoroutine(GameCoroutine());
        firstUpdateLogicPerformed = true;
    }

    private IEnumerator GameCoroutine()
    {
        CharacterSO characterSO = PlayerCharacterManager.Instance.CharacterSO;
        int stageNumber = GeneralStagesManager.Instance.CurrentStageNumber;
        int roundNumber = GeneralStagesManager.Instance.CurrentRoundNumber;

        bool gameEnded = false;

        InitializeState(State.StartingGame);
        GeneralStagesManager.Instance.InitializeToCurrentStage();

        yield return new WaitForSeconds(startingGameTimer);

        while (!gameEnded)
        {
            #region PreCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PreCombat))
            {
                ChangeState(State.Dialogue);

                yield return new WaitForSeconds(dialogueInterval);
                dialogueConcluded = false;
                DialogueTriggerHandler.Instance.PlayDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PreCombat);
                yield return new WaitUntil(() => dialogueConcluded);
                dialogueConcluded = false;
                yield return new WaitForSeconds(dialogueInterval);
            }
            #endregion

            #region Shop/AbilityUpgrade Logic
            if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage())
            {
                ChangeState(State.Upgrade);
                abilityUpgradeClosed = false;
                AbilityUpgradeOpeningManager.Instance.OpenAbilityUpgrade();
                yield return new WaitUntil(() => abilityUpgradeClosed);
                abilityUpgradeClosed = false;

            }
            else
            {
                ChangeState(State.Shop);
                shopClosed = false;
                ShopOpeningManager.Instance.OpenShop();
                yield return new WaitUntil(() => shopClosed);
                shopClosed = false;
            }
            #endregion

            #region BeginningCombat Logic
            ChangeState(State.BeginningCombat);
            yield return new WaitForSeconds(roundStartingTime);
            #endregion

            #region Combat Logic
            ChangeState(State.Combat);
            roundEnded = false;
            GeneralStagesManager.Instance.StartCurrentRound();
            yield return new WaitUntil(() => roundEnded);
            roundEnded = false;
            #endregion

            #region LoadNextRound Logic
            if (!GeneralStagesManager.Instance.LastCompletedStageAndRoundNumberAreLasts()) //Load Next Round&Stage and Save Data
            {
                GeneralStagesManager.Instance.LoadNextRoundAndStage();
                TriggerDataSave();
            }
            #endregion

            #region EndingCombat Logic
            ChangeState(State.EndingCombat);
            yield return new WaitForSeconds(roundEndingTime);
            #endregion

            #region PostCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PostCombat))
            {
                ChangeState(State.Dialogue);

                yield return new WaitForSeconds(dialogueInterval);
                dialogueConcluded = false;
                DialogueTriggerHandler.Instance.PlayDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PostCombat);
                yield return new WaitUntil(() => dialogueConcluded);
                dialogueConcluded = false;
                yield return new WaitForSeconds(dialogueInterval);
            }
            #endregion

            #region Win Logic
            if (GeneralStagesManager.Instance.LastCompletedStageAndRoundNumberAreLasts())
            {
                ChangeState(State.Win);
                gameEnded = true;
                break;
            }
            #endregion

            #region ChangeStage Logic
            if (GeneralStagesManager.Instance.LastCompletedRoundIsLastFromStage())
            {
                ChangeState(State.BeginningChangingStage);
                yield return new WaitForSeconds(changeStateStartingTimer);
                GeneralStagesManager.Instance.ChangeToCurrentStage();
                ChangeState(State.EndingChangingStage);
                yield return new WaitForSeconds(changeStateEndingTimer);
            }
            #endregion

            #region UpdateStageRoundValues
            stageNumber = GeneralStagesManager.Instance.CurrentStageNumber;
            roundNumber = GeneralStagesManager.Instance.CurrentRoundNumber;
            #endregion
        }
    }

    #endregion

    private void TriggerDataSave() => OnTriggerDataSave?.Invoke(this, EventArgs.Empty);

    #region Subscriptions
    private void ShopOpeningManager_OnShopClose(object sender, EventArgs e)
    {
        shopClosed = true;
    }

    private void ShopOpeningManager_OnShopCloseImmediately(object sender, EventArgs e)
    {
        shopClosed = true;
    }

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeClose(object sender, EventArgs e)
    {
        abilityUpgradeClosed = true;
    }

    private void AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately(object sender, EventArgs e)
    {
        abilityUpgradeClosed = true;
    }

    private void GeneralStagesManager_OnRoundEnd(object sender, GeneralStagesManager.OnRoundEventArgs e)
    {
        roundEnded = true;
    }

    private void DialogueManager_OnGeneralDialogueConcluded(object sender, EventArgs e)
    {
        dialogueConcluded = true;
    }

    #endregion
}
