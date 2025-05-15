using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyIdentifier enemyIdentifier;
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("Settings")]
    [SerializeField] private bool isSpawning;

    public bool IsSpawning => isSpawning;

    public static event EventHandler<OnEnemySpawnEventArgs> OnAnyEnemySpawnStart;
    public static event EventHandler<OnEnemySpawnEventArgs> OnAnyEnemySpawnComplete;

    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnStart;
    public event EventHandler<OnEnemySpawnEventArgs> OnEnemySpawnComplete;

    public class OnEnemySpawnEventArgs
    {
        public EnemySO enemySO;
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        isSpawning = true;

        OnAnyEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnEnemySpawnStart?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });

        float spawningTimer = 0f;

        while (spawningTimer < enemyIdentifier.EnemySO.spawnDuration)
        {
            if (!enemyHealth.IsAlive())
            {
                isSpawning = false;
                yield break;
            }

            spawningTimer += Time.deltaTime;
            yield return null;
        }

        OnAnyEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });
        OnEnemySpawnComplete?.Invoke(this, new OnEnemySpawnEventArgs { enemySO = enemyIdentifier.EnemySO });

        isSpawning = false;
    } 
}

