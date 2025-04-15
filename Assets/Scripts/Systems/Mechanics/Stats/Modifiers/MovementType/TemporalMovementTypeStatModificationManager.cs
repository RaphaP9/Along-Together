using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalMovementTypeStatModificationManager : MovementTypeStatModificationManager
{
    public static TemporalMovementTypeStatModificationManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one TemporalMovementTypeStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        //LoadFromRuntimeData
    }
}

