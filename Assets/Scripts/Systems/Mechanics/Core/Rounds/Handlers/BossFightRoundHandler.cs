using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightRoundHandler : RoundHandler
{
    public static BossFightRoundHandler Instance { get; private set; }

    [Header("Runtime Filled")]
    [SerializeField] private BossFightRoundSO currentBossFightRound;
    [SerializeField] protected float currentRoundElapsedTime;

    public static event EventHandler<OnBossFightRoundEventArgs> OnBossFightRoundStart;
    public static event EventHandler<OnBossFightRoundEventArgs> OnBossFightRoundCompleted;

    public class OnBossFightRoundEventArgs : EventArgs
    {
        public BossFightRoundSO bossFightRoundSO;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossFightRoundHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
