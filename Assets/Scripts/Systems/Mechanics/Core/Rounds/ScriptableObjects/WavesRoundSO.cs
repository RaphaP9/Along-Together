using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWavesRoundSO", menuName = "ScriptableObjects/Rounds/WavesRound")]
public class WavesRoundSO : RoundSO
{
    [Header("Timed Round Settings")]
    [Range(1f, 10f)] public float waveSpawnInterval;
    public List<EnemyWave> enemyWaves;

    public override RoundType GetRoundType() => RoundType.Waves;
}
