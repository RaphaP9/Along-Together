using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Continue")]
    [SerializeField] private Button continueButton;
    [SerializeField] private TransitionType continueTransitionType;
    [SerializeField] private string continueScene;

    [Header("NewGame")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private TransitionType newGameTransitionType;
    [SerializeField] private string newGameScene;

    [Header("Options")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private TransitionType optionsTransitionType;
    [SerializeField] private string optionsScene;

    [Header("Credits")]
    [SerializeField] private Button creditsButton;
    [SerializeField] private TransitionType creditsTransitionType;
    [SerializeField] private string creditsScene;

    [Header("Other")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button deleteDataButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }
    private void Start()
    {
        CheckContiueButtonAvailable();
    }


    private void InitializeButtonsListeners()
    {
        continueButton.onClick.AddListener(ContinueGame);
        newGameButton.onClick.AddListener(StartNewGame);
        optionsButton.onClick.AddListener(LoadOptionsScene);
        creditsButton.onClick.AddListener(LoadCreditsScene);
        quitButton.onClick.AddListener(QuitGame);
        deleteDataButton.onClick.AddListener(DeleteData);
    }

    private void CheckContiueButtonAvailable()
    {
        if (!DataUtilities.HasSavedRunData()) SetContinueButton(false);
        else SetContinueButton(true);
    }

    private void SetContinueButton(bool enable)
    {
        if (enable) continueButton.gameObject.SetActive(true);
        else continueButton.gameObject.SetActive(false);
    }

    private void ContinueGame() => ScenesManager.Instance.TransitionLoadTargetScene(continueScene, continueTransitionType);

    private void StartNewGame()
    {
        DataUtilities.WipeRunData();
        LoadNewGameScene();
    }

    private void LoadNewGameScene() => ScenesManager.Instance.TransitionLoadTargetScene(newGameScene, newGameTransitionType);
    private void LoadOptionsScene() => ScenesManager.Instance.TransitionLoadTargetScene(optionsScene, optionsTransitionType);
    private void LoadCreditsScene() => ScenesManager.Instance.TransitionLoadTargetScene(creditsScene, creditsTransitionType);
    private void DeleteData()
    {
        DataUtilities.WipeAllData();
        CheckContiueButtonAvailable();
    }
    private void QuitGame() => ScenesManager.Instance.QuitGame();
}
