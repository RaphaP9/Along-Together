using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentMaxHealthStatModificationManager : MaxHealthStatModificationManager
{
    public static PermanentMaxHealthStatModificationManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PermanentMaxHealthStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        //LoadFromRuntimeData
    }
}