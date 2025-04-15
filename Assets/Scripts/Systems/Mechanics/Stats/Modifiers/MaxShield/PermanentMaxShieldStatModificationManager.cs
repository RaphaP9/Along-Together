using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentMaxShieldStatModificationManager : MaxShieldStatModificationManager
{
    public static PermanentMaxShieldStatModificationManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PermanentShieldStatModificationManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        //LoadFromRuntimeData
    }
}
