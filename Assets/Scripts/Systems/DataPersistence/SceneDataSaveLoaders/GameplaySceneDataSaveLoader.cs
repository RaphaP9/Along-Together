using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneDataSaveLoader : MonoBehaviour
{
    public static GameplaySceneDataSaveLoader Instance {  get; private set; }

    private void Awake()
    {
        SetSingleton();
        GeneralDataSaveLoader.Instance.CompleteDataLoad();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        GeneralDataSaveLoader.Instance.CompleteDataSave();
    }
}
