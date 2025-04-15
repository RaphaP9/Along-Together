using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNumericStatsSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        RuntimeRunData.RuntimeNumericStats = data.numericStats;
    }

    public void SaveData(ref RunData data)
    {
        data.numericStats = RuntimeRunData.RuntimeNumericStats;
    }
}
