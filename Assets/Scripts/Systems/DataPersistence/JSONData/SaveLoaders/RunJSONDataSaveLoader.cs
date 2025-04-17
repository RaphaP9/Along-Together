using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RunJSONDataSaveLoader : JSONDataSaveLoader<RunData>
{
    public override void LoadData(RunData data)
    {
        SessionRunDataContainer.Instance.RunData = data;
    }

    public override void SaveData(ref RunData data)
    {
        data = SessionRunDataContainer.Instance.RunData;
    }
}
