using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneDataSaveLoader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LoadMode awakeLoadMode;
    [SerializeField] private SaveMode applicationQuitSaveMode; //Only Unity Editor

    //CompleteDataLoad/Save performs both JSON and session data operations
    private enum LoadMode {CompleteDataLoad, InjectionFromContainers, NoLoad}
    private enum SaveMode {CompleteDataSave, ExtractionToContainers, NoSave}

    protected virtual void Awake()
    {
        SetSingleton();
        HandleDataLoadOnAwake();
    }

    protected abstract void SetSingleton();

    private void HandleDataLoadOnAwake() //Synchronous Methods!
    {
        switch (awakeLoadMode)
        {
            case LoadMode.CompleteDataLoad:
                GeneralDataSaveLoader.Instance.CompleteDataLoad(); // Loads All JSON Data and Injects To Containers
                break;
            case LoadMode.InjectionFromContainers:
            default:
                GeneralDataSaveLoader.Instance.InjectAllDataFromContainers();
                break;
            case LoadMode.NoLoad:
                break;
        }
    }

    private void HandleDataSaveOnQuit() //Synchronous Methods!
    {
        switch (applicationQuitSaveMode)
        {
            case SaveMode.CompleteDataSave:
                GeneralDataSaveLoader.Instance.CompleteDataSave(); // Extracts All Data To Containers and Saves ALL JSON Data
                break;
            case SaveMode.ExtractionToContainers:
            default:
                GeneralDataSaveLoader.Instance.ExtractAllDataToContainers();
                break;
            case SaveMode.NoSave:
                break;
        }
    }

    private void OnApplicationQuit()
    {
        HandleDataSaveOnQuit();
    }
}
