using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPointsManager : MonoBehaviour
{
    public static EnemySpawnPointsManager Instance { get; private set; }

    [Header("Lists")]
    [SerializeField] private List<SpawnPointHandler> enemySpawnPoints;

    [Header("Settings")]
    [SerializeField, Range(3f, 10f)] private float minDistanceToPlayer;
    [SerializeField, Range(5f, 20f)] private float maxDistanceToPlayer;

    [Header("Debug")]
    [SerializeField] private Color gizmosColor;
    [SerializeField] private bool debug;

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
            Debug.LogWarning("There is more than one EnemySpawnPointValidator instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region SpawnPoint Validation


    public Transform GetRandomValidSpawnPoint()
    {
        List<Transform> enabledSpawnPoints = GetEnabledSpawnPoints(enemySpawnPoints);
        List<Transform> validSpawnPoints = FilterValidEnemySpawnPointsByMinMaxDistanceRange(enabledSpawnPoints, minDistanceToPlayer, maxDistanceToPlayer);
        Transform chosenSpawnPoint = ChooseRandomEnemySpawnPoint(validSpawnPoints);

        return chosenSpawnPoint;
    }

    private List<Transform> GetEnabledSpawnPoints(List<SpawnPointHandler> enemySpawnPointsPool)
    {
        List<Transform> enabledSpawnPoints = new List<Transform>();

        foreach(SpawnPointHandler spawnPoint in enemySpawnPointsPool)
        {
            if (spawnPoint.IsEnabled) enabledSpawnPoints.Add(spawnPoint.transform);
        }

        return enabledSpawnPoints;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinMaxDistanceRange(List<Transform> enemySpawnPointsPool, float minDistance, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> FilterValidEnemySpawnPointsByMinDistanceRange(List<Transform> enemySpawnPointsPool, float minDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMinDistanceRange(enemySpawnPoint, minDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private List<Transform> ChooseValidEnemySpawnPointsByMaxDistanceRange(List<Transform> enemySpawnPointsPool, float maxDistance)
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform enemySpawnPoint in enemySpawnPointsPool)
        {
            if (!EnemySpawnPointOnMaxDistanceRange(enemySpawnPoint, maxDistance)) continue;

            validSpawnPoints.Add(enemySpawnPoint);
        }

        return validSpawnPoints;
    }

    private Transform ChooseRandomEnemySpawnPoint(List<Transform> enemySpawnPointsPool)
    {
        Transform enemySpawnPoint = GeneralUtilities.ChooseRandomElementFromList(enemySpawnPointsPool);
        return enemySpawnPoint;
    }
    private bool EnemySpawnPointOnMinDistanceRange(Transform enemySpawnPoint, float minDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform)) > minDistance) return true;
        return false;
    }

    private bool EnemySpawnPointOnMaxDistanceRange(Transform enemySpawnPoint, float maxDistance)
    {
        if (Vector2.Distance(GeneralUtilities.TransformPositionVector2(enemySpawnPoint), GeneralUtilities.TransformPositionVector2(PlayerTransformRegister.Instance.PlayerTransform)) < maxDistance) return true;
        return false;
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        Vector3 position = Vector3.zero;

        if(PlayerTransformRegister.Instance != null && PlayerTransformRegister.Instance.PlayerTransform != null)
        {
            position = PlayerTransformRegister.Instance.PlayerTransform.position;   
        }

        Gizmos.DrawWireSphere(position, minDistanceToPlayer);
        Gizmos.DrawWireSphere(position, maxDistanceToPlayer);
    }
}
