using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetStatsSaveLoader : MonoBehaviour, IDataSaveLoader<GameData>
{
    public void LoadData(GameData data)
    {
        RuntimeGameData.RuntimeAssetStats = data.assetStats;
    }

    public void SaveData(ref GameData data)
    {
        data.assetStats = RuntimeGameData.RuntimeAssetStats;
    }
}
