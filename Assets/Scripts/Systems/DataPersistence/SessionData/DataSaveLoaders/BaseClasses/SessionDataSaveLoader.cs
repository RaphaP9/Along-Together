using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SessionDataSaveLoader : MonoBehaviour
{
    public abstract void LoadRuntimeData();
    public abstract void SaveRuntimeData();
}
