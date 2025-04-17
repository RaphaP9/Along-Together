using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SessionDataSaveLoader : MonoBehaviour
{
    protected virtual void Awake()
    {
        LoadRuntimeData();
    }

    protected abstract void LoadRuntimeData();
    protected abstract void SaveRuntimeData();
}
