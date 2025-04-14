using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasEmbeddedStats 
{
    public List<NumericEmbeddedStat> GetNumericEmbeddedStats();
    public List<AssetEmbeddedStat> GetAssetEmbeddedStats();
}
