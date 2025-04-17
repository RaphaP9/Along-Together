using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualJSONDataSaveLoader : JSONDataSaveLoader<PerpetualData>
{ 
    public override void LoadData(PerpetualData data)
    {
        SessionPerpetualDataContainer.Instance.PerpetualData = data;
    }

    public override void SaveData(ref PerpetualData data)
    {
        data = SessionPerpetualDataContainer.Instance.PerpetualData;
    }
}
