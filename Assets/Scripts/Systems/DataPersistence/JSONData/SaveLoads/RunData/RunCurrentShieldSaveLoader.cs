using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCurrentShieldSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        SessionDataManager.Instance.RunData.currentShield = data.currentShield;
    }

    public void SaveData(ref RunData data)
    {
        data.currentShield = SessionDataManager.Instance.RunData.currentShield;
    }
}
