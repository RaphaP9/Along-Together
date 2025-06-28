using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralSceneSettings : MonoBehaviour
{
    public static GeneralSceneSettings Instance { get; private set; }

    #region Initialization
    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogWarning("There is more than one GeneralSceneSettings instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    #endregion
}
