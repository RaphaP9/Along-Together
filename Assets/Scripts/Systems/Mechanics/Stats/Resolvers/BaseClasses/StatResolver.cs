using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatResolver : MonoBehaviour
{
    protected virtual void Awake()
    {
        SetSingleton();
    }

    protected abstract void SetSingleton();
}
