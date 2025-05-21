using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStagesManager : MonoBehaviour
{
    public static GeneralStagesManager Instance {  get; private set; }

    [Header("Lists")]
    [SerializeField] private List<StageGroup> stagesGroups;

    [Header("Runtime Filled")]
    [SerializeField] private StageSO currentStage;
    [SerializeField] private RoundSO currentRound;

    public List<StageGroup> StagesGroups => stagesGroups;
    public StageSO CurrentStage => currentStage;
    public RoundSO CurrentRound => currentRound;

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
            Debug.LogWarning("There is more than one GeneralStagesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public int GetStagesCount() => stagesGroups.Count;
}

[System.Serializable]
public class StageGroup
{
    public StageSO stageSO;
    public Transform playerSpawnPoint;
    public StageSpawnPointsHandler spawnPointsHandler;
}