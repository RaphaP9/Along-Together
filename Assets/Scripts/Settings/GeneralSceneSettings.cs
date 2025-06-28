using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSceneSettings : MonoBehaviour
{
    public static GeneralSceneSettings Instance { get; private set; }

    [Header("Starting Scene Settings")]
    [SerializeField] private string regularStartingScene;
    [SerializeField] private TransitionType regularStartingSceneTransitionType;
    [Space]
    [SerializeField] private string firstSessionStartingScene;
    [SerializeField] private TransitionType firstSessionStartingSceneTransitionType;

    [Header("Character Specific Scenes")]
    [SerializeField] private List<CharacterSpecificScenes> characterSpecificScenes;

    #region Initialization
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
            //Debug.LogWarning("There is more than one GeneralSceneSettings instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion

    public bool ShouldTransitionToFirstSessionStartingScene() => SessionPerpetualDataContainer.Instance.PerpetualData.timesEnteredGame <= 0;

    public void TransitionToStartingScene()
    {
        if (ShouldTransitionToFirstSessionStartingScene()) ScenesManager.Instance.TransitionLoadTargetScene(firstSessionStartingScene, firstSessionStartingSceneTransitionType);
        else ScenesManager.Instance.TransitionLoadTargetScene(regularStartingScene, regularStartingSceneTransitionType);
    }
}

[System.Serializable]
public class CharacterSpecificScenes
{
    public CharacterSO characterSO;
    [Space]
    public string firstRunScene;
    public TransitionType firstRunTransitionType;
    [Space]
    public string firstWinScene;
    public TransitionType firstWinTransitionType;
}
