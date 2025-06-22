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

    [Header("Settings - Tutorial")]
    [SerializeField] private bool tutorializedRun;

    [Header("Settings - Timers - Tutorial")]
    [SerializeField, Range(0f, 2f)] private float tutorializedActionInterval;
    [SerializeField, Range(0f, 2f)] private float betweenTutorializedActionsTime;

    [Header("Debug")]
    [SerializeField] private bool ignoreGameFlow;

    public static event EventHandler OnTriggerDataSave;

    //Monologue is considered non GameState intrusive, can happen on combat,etc
    public enum State {StartingGame, BeginningCombat, Combat, EndingCombat, Shop, Upgrade, BeginningChangingStage, EndingChangingStage, Cinematic, Dialogue, Lose, Win, Tutorial} 

    public State GameState => state;
    public bool TutorializedRun => tutorializedRun;

    public static event EventHandler<OnStateChangeEventArgs> OnStateChanged;
    public static event EventHandler<OnStateInitializedEventArgs> OnStateInitialized;

    public static event EventHandler OnGameLost;
    public static event EventHandler OnGameWon;

    #region Flags
    private bool firstUpdateLogicPerformed = false;
    private bool dialogueConcluded = false;
    private bool shopClosed = false;
    private bool abilityUpgradeClosed = false;
    private bool roundEnded = false;
    private bool tutorializedActionClosed = false;
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

        PlayerHealth.OnAnyPlayerDeath += PlayerHealth_OnAnyPlayerDeath;

        ////////

        TutorialOpeningManager.OnTutorializedActionClosed += TutorialOpeningManager_OnTutorializedActionClose;
    }

    private void OnDisable()
    {
        ShopOpeningManager.OnShopClose -= ShopOpeningManager_OnShopClose;
        ShopOpeningManager.OnShopCloseImmediately -= ShopOpeningManager_OnShopCloseImmediately;

        AbilityUpgradeOpeningManager.OnAbilityUpgradeClose -= AbilityUpgradeOpeningManager_OnAbilityUpgradeClose;
        AbilityUpgradeOpeningManager.OnAbilityUpgradeCloseImmediately -= AbilityUpgradeOpeningManager_OnAbilityUpgradeCloseImmediately;

        GeneralStagesManager.OnRoundEnd -= GeneralStagesManager_OnRoundEnd;

        DialogueManager.OnGeneralDialogueConcluded -= DialogueManager_OnGeneralDialogueConcluded;

        PlayerHealth.OnAnyPlayerDeath -= PlayerHealth_OnAnyPlayerDeath;

        ////////

        TutorialOpeningManager.OnTutorializedActionClosed -= TutorialOpeningManager_OnTutorializedActionClose;
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

        if (ignoreGameFlow)
        {
            InitializeState(State.Combat);
        }
        else if (tutorializedRun)
        {
            StartCoroutine(TutorializedGameCoroutine());
        }
        else 
        {
            StartCoroutine(RegularGameCoroutine());
        }

        firstUpdateLogicPerformed = true;
    }

    private IEnumerator RegularGameCoroutine()
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
                yield return StartCoroutine(DialogueCoroutine(characterSO, stageNumber, roundNumber, DialogueChronology.PreCombat));

            }
            #endregion

            #region Shop/AbilityUpgrade Logic
            if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage())
            {
                #region AbilityUpgrade Logic
                if (AbilityUpgradeCardsGenerator.Instance.CanGenerateNextLevelActiveAbilityVariantCards()) //Only Open AbilityUpgradeUI if can upgrade an ability
                {
                    yield return StartCoroutine(AbilityUpgradeCoroutine());
                }
                else
                {
                    yield return StartCoroutine(ShopCoroutine());
                }
                #endregion
            }
            else
            {
                yield return StartCoroutine(ShopCoroutine());
            }
            #endregion

            #region CompleteCombat Logic
            yield return StartCoroutine(CompleteCombatCoroutine());
            #endregion

            #region PostCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PostCombat))
            {
                yield return StartCoroutine(DialogueCoroutine(characterSO,stageNumber,roundNumber, DialogueChronology.PostCombat));
            }
            #endregion

            #region Win Logic
            if (GeneralStagesManager.Instance.LastCompletedStageAndRoundNumberAreLasts())
            {
                gameEnded = true;
                WinGame();
                break;
            }
            #endregion

            #region ChangeStage Logic
            if (GeneralStagesManager.Instance.LastCompletedRoundIsLastFromStage())
            {
                yield return StartCoroutine(ChangeStageCoroutine());
            }
            #endregion

            #region Update Stage&Round Values In-Between Rounds
            stageNumber = GeneralStagesManager.Instance.CurrentStageNumber;
            roundNumber = GeneralStagesManager.Instance.CurrentRoundNumber;
            #endregion
        }
    }

    private IEnumerator TutorializedGameCoroutine()
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
                yield return StartCoroutine(DialogueCoroutine(characterSO, stageNumber, roundNumber, DialogueChronology.PreCombat));

            }
            #endregion

            #region Shop/AbilityUpgrade Logic
            if (GeneralStagesManager.Instance.CurrentRoundIsFirstFromCurrentStage() && !GeneralStagesManager.Instance.CurrentStageAndRoundAreFirsts()) //Skip AbilityUpgrade if First Stage&Round
            {
                #region AbilityUpgrade Logic
                if (AbilityUpgradeCardsGenerator.Instance.CanGenerateNextLevelActiveAbilityVariantCards()) //Only Open AbilityUpgradeUI if can upgrade an ability
                {
                    yield return StartCoroutine(AbilityUpgradeCoroutine());
                }
                else
                {
                    yield return StartCoroutine(ShopCoroutine());
                }
                #endregion
            }
            else if (GeneralStagesManager.Instance.CurrentStageAndRoundAreValues(1, 2)) //Open it on Round 1-2 with respective tutorial Panel
            {
                //Open AbilityUpgradeTutorialPanel (dont wait for condition)
                Debug.Log("AbilityUpgradeTutorialPanel");
                #region AbilityUpgrade Logic
                if (AbilityUpgradeCardsGenerator.Instance.CanGenerateNextLevelActiveAbilityVariantCards()) //Only Open AbilityUpgradeUI if can upgrade an ability
                {
                    yield return StartCoroutine(AbilityUpgradeCoroutine());
                }
                else
                {
                    yield return StartCoroutine(ShopCoroutine());
                }
                #endregion
            }
            else if (GeneralStagesManager.Instance.CurrentStageAndRoundAreValues(1, 3)) //Open shop on stage 1-3 with respective tutorial Panel
            {
                //Open ShopTutorialPanel (dont wait for condition)
                Debug.Log("ShopTutorialPanel");
                yield return StartCoroutine(ShopCoroutine());
            }
            else if (!GeneralStagesManager.Instance.CurrentStageAndRoundAreFirsts()) //Skip Shop if First Stage&Round
            {
                yield return StartCoroutine(ShopCoroutine());
            }
            #endregion


            if (GeneralStagesManager.Instance.CurrentStageAndRoundAreFirsts()) //If round 1-1
            {
                yield return StartCoroutine(TutorializedActionCoroutine(TutorializedAction.Movement));
                yield return new WaitForSeconds(betweenTutorializedActionsTime);
                yield return StartCoroutine(TutorializedActionCoroutine(TutorializedAction.Attack));
            }

            if (GeneralStagesManager.Instance.CurrentStageAndRoundAreValues(1,2)) //if round 1-2
            {
                //Open Tutorial Panel for Ability Casting(wait for condition)
                Debug.Log("Ability Casting Panel");
            }

            #region CompleteCombat Logic
            yield return StartCoroutine(CompleteCombatCoroutine());
            #endregion

            #region PostCombat Dialogue Logic
            if (DialogueTriggerHandler.Instance.ExistDialogueWithConditions(characterSO, stageNumber, roundNumber, DialogueChronology.PostCombat))
            {
                yield return StartCoroutine(DialogueCoroutine(characterSO, stageNumber, roundNumber, DialogueChronology.PostCombat));
            }
            #endregion

            #region Win Logic
            if (GeneralStagesManager.Instance.LastCompletedStageAndRoundNumberAreLasts())
            {
                gameEnded = true;
                WinGame();
                break;
            }
            #endregion

            #region ChangeStage Logic
            if (GeneralStagesManager.Instance.LastCompletedRoundIsLastFromStage())
            {
                yield return StartCoroutine(ChangeStageCoroutine());
            }
            #endregion

            #region Update Stage&Round Values In-Between Rounds
            stageNumber = GeneralStagesManager.Instance.CurrentStageNumber;
            roundNumber = GeneralStagesManager.Instance.CurrentRoundNumber;
            #endregion
        }
    }

    #endregion

    #region Utility Coroutines
    private IEnumerator AbilityUpgradeCoroutine()
    {
        ChangeState(State.Upgrade);
        abilityUpgradeClosed = false;
        AbilityUpgradeOpeningManager.Instance.OpenAbilityUpgrade();
        yield return new WaitUntil(() => abilityUpgradeClosed);
        abilityUpgradeClosed = false;
    }

    private IEnumerator ShopCoroutine()
    {
        ChangeState(State.Shop);
        shopClosed = false;
        ShopOpeningManager.Instance.OpenShop();
        yield return new WaitUntil(() => shopClosed);
        shopClosed = false;
    }

    private IEnumerator ChangeStageCoroutine()
    {
        ChangeState(State.BeginningChangingStage);
        yield return new WaitForSeconds(changeStateStartingTimer);
        GeneralStagesManager.Instance.ChangeToCurrentStage();
        ChangeState(State.EndingChangingStage);
        yield return new WaitForSeconds(changeStateEndingTimer);
    }

    private IEnumerator DialogueCoroutine(CharacterSO characterSO, int stageNumber, int roundNumber, DialogueChronology dialogueChronology)
    {
        ChangeState(State.Dialogue);

        yield return new WaitForSeconds(dialogueInterval);

        dialogueConcluded = false;
        DialogueTriggerHandler.Instance.PlayDialogueWithConditions(characterSO, stageNumber, roundNumber, dialogueChronology);
        yield return new WaitUntil(() => dialogueConcluded);
        dialogueConcluded = false;

        yield return new WaitForSeconds(dialogueInterval);
    }

    private IEnumerator CompleteCombatCoroutine()
    {
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
    }

    private IEnumerator TutorializedActionCoroutine(TutorializedAction tutorializedAction)
    {
        ChangeState(State.Tutorial);

        yield return new WaitForSeconds(tutorializedActionInterval);

        tutorializedActionClosed = false;
        TutorialOpeningManager.Instance.OpenTutorializedAction(tutorializedAction);
        yield return new WaitUntil(() => tutorializedActionClosed);
        tutorializedActionClosed = false;

        yield return new WaitForSeconds(tutorializedActionInterval);
    }
    #endregion

    #region  Win & Lose
    private void WinGame()
    {
        ChangeState(State.Win);
        OnGameWon?.Invoke(this, EventArgs.Empty);
    }

    private void LoseGame()
    {
        StopAllCoroutines(); //Stop Game Coroutine
        SetGameState(State.Lose);
        OnGameLost?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    private void TriggerDataSave() => OnTriggerDataSave?.Invoke(this, EventArgs.Empty);
    public void SetTutorializedRun(bool tutorializedRun) => this.tutorializedRun = tutorializedRun;

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

    private void PlayerHealth_OnAnyPlayerDeath(object sender, EntityHealth.OnEntityDeathEventArgs e)
    {
        LoseGame();
    }

    //////

    private void TutorialOpeningManager_OnTutorializedActionClose(object sender, TutorialOpeningManager.OnTutorializedActionEventArgs e)
    {
        tutorializedActionClosed = true;
    }
    #endregion
}
