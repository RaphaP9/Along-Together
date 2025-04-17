using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualJSONDataSaveLoader : JSONDataSaveLoader<PerpetualData>
{
    [Header("Data Containers")]
    [SerializeField] private SessionPerpetualDataContainer sessionPerpetualDataContainer;

    public override void LoadData(PerpetualData data)
    {
        sessionPerpetualDataContainer.PerpetualData = data;
    }

    public override void SaveData(ref PerpetualData data)
    {
        data = sessionPerpetualDataContainer.PerpetualData;
    }


}
