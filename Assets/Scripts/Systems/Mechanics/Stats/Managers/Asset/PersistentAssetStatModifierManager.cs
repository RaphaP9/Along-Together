using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAssetStatModifierManager : AssetStatModifierManager
{
    public static PersistentAssetStatModifierManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PersistentAssetStatModifierManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override void LoadRuntimeData()
    {
        //
    }
}

