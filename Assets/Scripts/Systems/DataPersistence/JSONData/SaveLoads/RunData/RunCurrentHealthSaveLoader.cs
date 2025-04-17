using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCurrentHealthSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        SessionDataManager.Instance.RunData.currentHealth = data.currentHealth;
    }

    public void SaveData(ref RunData data)
    {
        data.currentHealth = SessionDataManager.Instance.RunData.currentHealth;
    }
}
