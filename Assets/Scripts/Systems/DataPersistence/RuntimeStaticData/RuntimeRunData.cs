using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuntimeRunData
{
    public static int CurrentHealth = 0;
    public static int CurrentShield = 0;

    public static List<DataPersistentAssetStat> RuntimeAssetStats = new();
    public static List<DataPersistentNumericStat> RuntimeNumericStats = new();

}
