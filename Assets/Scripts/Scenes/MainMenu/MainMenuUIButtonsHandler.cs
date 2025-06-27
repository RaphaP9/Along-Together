using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("NewGame")]
    [SerializeField] private Button newGameButton;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

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
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(LoadContinueGameScene);
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

    private void StartNewGame() => MainMenuNextScenesManager.Instance.StartNewGame();
    private void LoadContinueGameScene() => MainMenuNextScenesManager.Instance.ContinueGame();
    private void LoadOptionsScene() => ScenesManager.Instance.TransitionLoadTargetScene(optionsScene, optionsTransitionType);
    private void LoadCreditsScene() => ScenesManager.Instance.TransitionLoadTargetScene(creditsScene, creditsTransitionType);

    private void DeleteData()
    {
        SessionRunDataContainer.Instance.ResetRunData(); //Reset the Run Data in Data Container
        SessionPerpetualDataContainer.Instance.ResetPerpetualData(); //Reset the Perpetual Data in Data Container

        DataUtilities.WipeAllData();

        CheckContiueButtonAvailable();
    }
    private void QuitGame() => ScenesManager.Instance.QuitGame();
}
