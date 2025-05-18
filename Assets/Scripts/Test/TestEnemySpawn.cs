using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemySO enemySO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnemySpawnerManager.Instance.SpawnEnemyOnValidRandomSpawnPoint(enemySO);
        }
    }
}
