using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalMaxShieldStatModificationManager : MaxShieldStatModificationManager
{
    public static TemporalMaxShieldStatModificationManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one TemporalMaxShieldStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        //LoadFromRuntimeData
    }
}
