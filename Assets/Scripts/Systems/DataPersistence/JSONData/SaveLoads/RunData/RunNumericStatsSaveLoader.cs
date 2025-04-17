using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunNumericStatsSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        SessionDataManager.Instance.RunData.numericStats = data.numericStats;
    }

    public void SaveData(ref RunData data)
    {
        data.numericStats = SessionDataManager.Instance.RunData.numericStats;
    }
}
