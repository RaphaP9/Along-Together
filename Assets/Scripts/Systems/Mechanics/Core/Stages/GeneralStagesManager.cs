using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStagesManager : MonoBehaviour
{
    public static GeneralStagesManager Instance {  get; private set; }

    [Header("Lists")]
    [SerializeField] private List<StageGroup> stagesGroups;

    [Header("Settings")]
    [SerializeField] private int startingStageNumber;
    [SerializeField] private int startingRoundNumber;

    [Header("Runtime Filled")]
    [SerializeField] private StageSO currentStage;
    [SerializeField] private RoundSO currentRound;
    [Space]
    [SerializeField] private int currentStageNumber;
    [SerializeField] private int currentRoundNumber;

    public List<StageGroup> StagesGroups => stagesGroups;
    public StageSO CurrentStage => currentStage;
    public RoundSO CurrentRound => currentRound;
    public int CurrentStageNumber => currentStageNumber;
    public int CurrentRoundNumber => currentRoundNumber;

    private const int FIRTS_STAGE_NUMBER = 1;
    private const int FIRST_ROUND_NUMBER = 1;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeStage();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GeneralStagesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeStage()
    {
        currentStageNumber = startingStageNumber <= 0? FIRTS_STAGE_NUMBER : startingStageNumber;
        currentRoundNumber = startingRoundNumber <= 0? FIRST_ROUND_NUMBER : startingRoundNumber;
    }

    public void SetStartingStageNumber(int setterStartingStageNumber) => startingStageNumber = setterStartingStageNumber;
    public void SetStartingRoundNumber(int setterStartingRoundNumber) => startingRoundNumber = setterStartingRoundNumber;

    public int GetStagesCount() => stagesGroups.Count;
}

[System.Serializable]
public class StageGroup
{
    public StageSO stageSO;
    public Transform playerSpawnPoint;
    public StageSpawnPointsHandler spawnPointsHandler;
}