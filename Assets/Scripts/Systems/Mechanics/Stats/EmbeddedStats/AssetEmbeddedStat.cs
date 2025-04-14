using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetEmbeddedStat : EmbeddedStat
{
    public AssetStatModificationType assetStatModificationType;
    public ScriptableObject asset;

    public override StatValueType GetStatValueType() => StatValueType.Asset;
}
