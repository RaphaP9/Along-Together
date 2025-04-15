using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeStatResolver : AssetStatResolver<MovementTypeSO>
{
    public static MovementTypeStatResolver Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one MovementTypeStatResolver instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    protected override AssetStatType GetAssetStatType() => AssetStatType.MovementType;
}