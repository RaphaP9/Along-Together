using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualNumericStatsSaveLoader : MonoBehaviour, IDataSaveLoader<PerpetualData>
{
    public void LoadData(PerpetualData data)
    {
        SessionDataManager.Instance.PerpetualData.numericStats = data.numericStats;
    }

    public void SaveData(ref PerpetualData data)
    {
        data.numericStats = SessionDataManager.Instance.PerpetualData.numericStats;
    }
}
