using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Instance { get; private set; }

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnEnemySpawnedEventArgs> OnEnemySpawned;
    public class OnEnemySpawnedEventArgs : EventArgs
    {
        public EnemySO enemySO;
        public Vector2 position;
    }

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
            Debug.LogWarning("There is more than one EnemySpawnerManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public Transform SpawnEnemyOnValidRandomSpawnPoint(EnemySO enemySO)
    {
        Transform chosenSpawnPoint = EnemySpawnPointsManager.Instance.GetRandomValidSpawnPoint();

        if(chosenSpawnPoint == null)
        {
            if (debug) Debug.Log("Chosen SpawnPoint is null. Spawn will be ignored");
            return null;
        }

        Transform spawnedEnemy = SpawnEnemyAtPosition(enemySO, chosenSpawnPoint.position);

        return spawnedEnemy;
    }

    public Transform SpawnEnemyAtPosition(EnemySO enemySO, Vector3 position)
    {
        if(enemySO.prefab == null)
        {
            if (debug) Debug.Log($"EnemySO with name {enemySO.entityName} does not contain an enemy prefab. Instantiation will be ignored.");
            return null;
        }

        Transform spawnedEnemy = Instantiate(enemySO.prefab, position, Quaternion.identity);

        OnEnemySpawned?.Invoke(this, new OnEnemySpawnedEventArgs { enemySO = enemySO, position = position });

        return spawnedEnemy;
    }

  
}
