using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNumericStatsSaveLoader : MonoBehaviour, IDataSaveLoader<GameData>
{
    public void LoadData(GameData data)
    {
        RuntimeGameData.RuntimeNumericStats = data.numericStats;
    }

    public void SaveData(ref GameData data)
    {
        data.numericStats = RuntimeGameData.RuntimeNumericStats;
    }
}
