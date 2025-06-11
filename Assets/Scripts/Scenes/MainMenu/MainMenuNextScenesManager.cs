using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNextScenesManager : MonoBehaviour
{
    public static MainMenuNextScenesManager Instance { get; private set; }

    [Header("Continue")]
    [SerializeField] private TransitionType continueTransitionType;
    [SerializeField] private string continueScene;

    [Header("Basic Run")]
    [SerializeField] private TransitionType basicNewGameTransitionType;
    [SerializeField] private string basicNewGameScene;

    [Header("Regular Run")]
    [SerializeField] private TransitionType regularNewGameTransitionType;
    [SerializeField] private string regularNewGameScene;

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
            Destroy(gameObject);
        }
    }

    public void ContinueGame()
    {
        ScenesManager.Instance.TransitionLoadTargetScene(continueScene, continueTransitionType); //Do not Delete Any Data
    }

    public void StartNewGame()
    {
        DataUtilities.WipeRunData(); //Delete JSON Run Data
        SessionRunDataContainer.Instance.ResetRunData(); //Reset the Run Data in Data Container

        string newGameScene;
        TransitionType newGameTransitionType;

        if (SessionPerpetualDataContainer.Instance.HasUnlockedCharacters())
        {
            newGameScene = regularNewGameScene;
            newGameTransitionType = regularNewGameTransitionType;
        }
        else
        {
            newGameScene = basicNewGameScene;
            newGameTransitionType = basicNewGameTransitionType;
        }

        ScenesManager.Instance.TransitionLoadTargetScene(newGameScene, newGameTransitionType); //Do not Delete Any Data
    }
}
