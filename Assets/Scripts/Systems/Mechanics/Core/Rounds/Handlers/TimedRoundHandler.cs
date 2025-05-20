using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedRoundHandler : RoundHandler
{
    public static TimedRoundHandler Instance { get; private set; }

    [Header("Timed Round Settings")]
    [SerializeField, Range(0f, 3f)] private float weightNormalizedIncreaseFactor; //When the wave normalized elapsed time is 1, each enemy weight has increased by totalWeight * weightNormalizedIncreaseFactor
    [SerializeField, Range(0f, 0.8f)] private float spawnIntervalNormalizedReductionFactor; //When the normalized elapsed time is 1, enemies spawn every baseSpawnTime * spawnTimeNormalizedReductionFactor

    [Header("Runtime Filled")]
    [SerializeField] private TimedRoundSO currentTimedRound;
    [SerializeField] protected float currentRoundDuration;
    [SerializeField] protected float currentRoundElapsedTime;

    public float CurrentRoundElapsedTime => currentRoundElapsedTime;
    public float CurrentRoundCountdown => currentRoundDuration - currentRoundElapsedTime;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one TimedRoundHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ClearCurrentRound();
        ResetCurrentRoundDuration();
        ResetCurrentRoundElapsedTime();
    }

    public void StartTimedRound(TimedRoundSO timedRoundSO)
    {
        if (currentTimedRound != null) return;

        OnRoundStartMethod(timedRoundSO);

        SetCurrentTimedRound(timedRoundSO);
        SetCurrentRoundDuration(timedRoundSO.duration);
        ResetCurrentRoundElapsedTime();

        StartCoroutine(StartRoundCoroutine(timedRoundSO));
    }

    private IEnumerator StartRoundCoroutine(TimedRoundSO timedRoundSO)
    {
        float roundElapsedTimer = 0f;
        float enemySpawnTimer = Mathf.Infinity; //To spawn an enemy inmediately after wave start, otherwise, set to 0

        while (roundElapsedTimer < timedRoundSO.duration)
        {
            roundElapsedTimer += Time.deltaTime;
            enemySpawnTimer += Time.deltaTime;

            float normalizedElapsedTime = Mathf.Clamp01(roundElapsedTimer / timedRoundSO.duration);

            if (enemySpawnTimer > RoundUtilties.GetTimedRoundDinamicSpawnInterval(timedRoundSO.baseSpawnInterval, spawnIntervalNormalizedReductionFactor, normalizedElapsedTime))
            {
                EnemySO enemyToSpawn = RoundUtilties.GetRandomDinamicEnemyByWeight(timedRoundSO.proceduralRoundEnemyGroups, weightNormalizedIncreaseFactor ,normalizedElapsedTime);
                EnemiesManager.Instance.SpawnEnemyOnValidRandomSpawnPoint(enemyToSpawn);

                enemySpawnTimer = 0f;
            }

            SetCurrentRoundElapsedTime(roundElapsedTimer);

            yield return null;
        }

        CompleteCurrentRound();
    }

    protected virtual void CompleteCurrentRound()
    {
        if (currentTimedRound == null) return;

        ClearCurrentRound();
        ResetCurrentRoundDuration();
        ResetCurrentRoundElapsedTime();

        OnRoundCompletedMethod(currentTimedRound);

        EnemiesManager.Instance.ExecuteAllActiveEnemies();
    }

    #region Set & Get

    protected float GetNormalizedRoundElapsedTime() => currentRoundElapsedTime / currentRoundDuration;
    protected void SetCurrentTimedRound(TimedRoundSO timedRoundSO) => currentTimedRound = timedRoundSO;
    protected void SetCurrentRoundDuration(float duration) => currentRoundDuration = duration;
    protected void SetCurrentRoundElapsedTime(float elapsedTime) => currentRoundElapsedTime = elapsedTime;

    protected void ClearCurrentRound() => currentTimedRound = null;
    protected void ResetCurrentRoundDuration() => currentRoundDuration = 0;
    protected void ResetCurrentRoundElapsedTime() => currentRoundElapsedTime = 0;
    #endregion 
}
