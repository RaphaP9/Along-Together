using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetStatResolver<T> : StatResolver where T : AssetStatSO
{
    [Header("Lists")]
    [SerializeField] private List<AssetStatModifierManager> assetStatModifierManagers;

    public List<AssetStatModifierManager> AssetStatModifierManagers => assetStatModifierManagers;

    public virtual List<T> ResolveStat(List<T> baseList)
    {
        List<T> statList = new List<T>(baseList);

        foreach (AssetStatModifierManager assetStatModifierManager in assetStatModifierManagers)
        {
            foreach(AssetStatModifier assetStatModifier in assetStatModifierManager.AssetStatModifiers)
            {
                if (assetStatModifier.assetStatType != GetAssetStatType()) continue;
                if (assetStatModifier.asset == null) continue;
                if (!CheckAssetIsT(assetStatModifier.asset)) continue;

                switch (assetStatModifier.assetStatModificationType)
                {
                    case AssetStatModificationType.Union:
                    default:
                        statList.Add(assetStatModifier.asset as T);
                        break;
                    case AssetStatModificationType.Replacement:
                        statList.Clear();
                        statList.Add(assetStatModifier.asset as T);
                        return statList;
                }
            }
        }

        return statList;
    }

    protected bool CheckAssetIsT(AssetStatSO assetStatSO) => assetStatSO is T;
    protected abstract AssetStatType GetAssetStatType();

  
}
