using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCurrentHealthSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        RuntimeRunData.CurrentHealth = data.currentHealth;
    }

    public void SaveData(ref RunData data)
    {
        data.currentHealth = RuntimeRunData.CurrentHealth;
    }
}
