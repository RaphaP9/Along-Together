using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGroupHandler : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<SpawnPointHandler> groupSpawnPoints;

    public List<SpawnPointHandler> GroupSpawnPoints => groupSpawnPoints;

    public void EnableSpawnPoints()
    {
        foreach(SpawnPointHandler spawnPoint in groupSpawnPoints)
        {
            spawnPoint.SetIsEnabled(true);
        }
    }

    public void DisableSpawnPoints()
    {
        foreach (SpawnPointHandler spawnPoint in groupSpawnPoints)
        {
            spawnPoint.SetIsEnabled(false);
        }
    }
}
