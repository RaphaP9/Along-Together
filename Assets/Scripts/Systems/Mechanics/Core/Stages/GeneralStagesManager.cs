using System;
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

    [Header("Settings - Starting/Ending timers")]
    [SerializeField, Range(2f,5f)] private float roundStartingTime;
    [SerializeField, Range(2f,5f)] private float roundEndingTime;

    [Header("Runtime Filled")]
    [SerializeField] private StageGroup currentStageGroup;
    [SerializeField] private RoundGroup currentRoundGroup;
    [SerializeField] private RoundSO currentRound;
    [Space]
    [SerializeField] private int currentStageNumber;
    [SerializeField] private int currentRoundNumber;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public List<StageGroup> StagesGroups => stagesGroups;
    public StageGroup CurrentStage => currentStageGroup;
    public RoundGroup CurrentRoundGroup => currentRoundGroup;
    public RoundSO CurrentRound => currentRound;

    public int CurrentStageNumber => currentStageNumber;
    public int CurrentRoundNumber => currentRoundNumber;


    public static event EventHandler<OnStageEventArgs> OnStageInitialized;

    public class OnStageEventArgs : EventArgs
    {
        public int stageNumber;
        public int roundNumber;
    }

    private const int FIRTS_STAGE_NUMBER = 1;
    private const int FIRST_ROUND_NUMBER = 1;


    #region Flags
    private bool currentRoundEnded = false;
    #endregion

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

        StageGroup stageGroup = LocateStageGroupByStageNumber(currentStageNumber);
        RoundGroup roundGroup = LocateRoundGroupByRoundNumber(currentStageNumber, currentRoundNumber);

        //Verify if currentValues do exist
        if (stageGroup == null)
        {
            currentStageNumber = FIRTS_STAGE_NUMBER;
            currentRoundNumber = FIRST_ROUND_NUMBER;

            if (debug) Debug.Log("Could not locate StageGroup on Initialization, reseting Stage & Round Numbers to Initials.");
        }
        else if (roundGroup == null)
        {
            currentStageNumber = FIRTS_STAGE_NUMBER;
            currentRoundNumber = FIRST_ROUND_NUMBER;

            if (debug) Debug.Log("Could not locate RoundGroup on Initialization, reseting Stage & Round Numbers to Initials.");
        }


        OnStageInitialized?.Invoke(this, new OnStageEventArgs { stageNumber = currentStageNumber, roundNumber = currentRoundNumber });
    }

    public void StartLoadedRound()
    {
       
    }

    #region UtilityMethods

    private StageGroup LocateStageGroupByStageNumber(int stageNumber)
    {
        if(stageNumber > stagesGroups.Count)
        {
            //if (debug) Debug.Log($"Stages are less than Stage Number: {stageNumber}. Returning null.");
            return null;
        }

        return stagesGroups[stageNumber - 1];
    }

    private RoundGroup LocateRoundGroupByRoundNumber(int stageNumber, int roundNumber)
    {
        StageGroup stageGroup = LocateStageGroupByStageNumber(stageNumber);
        
        if (stageGroup == null)
        {
            //if (debug) Debug.Log($"As StageGroup is null, RoundGroup can not be found. Returning null.");
            return null;
        }

        if (roundNumber > stageGroup.stageSO.roundGroups.Count)
        {
            //if (debug) Debug.Log($"Rounds are less than Round Number: {roundNumber}. Returning null.");
            return null;
        }

        return stageGroup.stageSO.roundGroups[roundNumber - 1];
    }

    private bool IsLastStageGroup(StageGroup stageGroup) => stageGroup == stagesGroups[^1];
    private bool IsLastRoundGroupFromStageGroup(StageSO stageSO, RoundGroup roundGroup) => roundGroup == stageSO.roundGroups[^1];

    private int GetNextStageNumberByStageAndRoundNumbers(int stageNumber, int roundNumber)
    {
        StageGroup stageGroup = LocateStageGroupByStageNumber(stageNumber);
        return 0;
    }

    #endregion

    #region Get & Set
    public void SetStartingStageNumber(int setterStartingStageNumber) => startingStageNumber = setterStartingStageNumber;
    public void SetStartingRoundNumber(int setterStartingRoundNumber) => startingRoundNumber = setterStartingRoundNumber;

    private void SetCurrentStageGroup(StageGroup stageGroup) => currentStageGroup = stageGroup;
    private void ClearCurrentStageGroup() => currentStageGroup = null;

    private void SetCurrentRoundGroup(RoundGroup roundGroup) => currentRoundGroup = roundGroup;
    private void ClearCurrentRoundGroup() => currentRoundGroup = null;

    private void SetCurrentRound(RoundSO round) => currentRound = round;
    private void ClearCurrentRound() => currentRound = null;

    public int GetStagesCount() => stagesGroups.Count;
    #endregion

}

[System.Serializable]
public class StageGroup
{
    public StageSO stageSO;
    public Transform playerSpawnPoint;
    public StageSpawnPointsHandler spawnPointsHandler;
}