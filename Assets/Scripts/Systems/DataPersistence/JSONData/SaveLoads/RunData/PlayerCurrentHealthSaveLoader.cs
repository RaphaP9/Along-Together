using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentHealthSaveLoader : MonoBehaviour, IDataSaveLoader<RunData>
{
    public void LoadData(RunData data)
    {
        //LoadTo RuntimeRunData
    }

    public void SaveData(ref RunData data)
    {
        //Also Save to RuntimeRunData
    }
}
