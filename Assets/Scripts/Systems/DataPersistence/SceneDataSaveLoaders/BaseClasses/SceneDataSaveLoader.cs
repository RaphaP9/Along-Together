using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneDataSaveLoader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LoadMode awakeLoadMode;
    [SerializeField] private bool allowDynamicLoad; //Only Session Data Loads Dinamically
    [Space]
    [SerializeField] private SaveMode dynamicSaveMode;
    [SerializeField] private SaveMode applicationQuitSaveMode;

    private enum LoadMode {CompleteDataLoad, JSONDataLoad, SessionDataLoad, NoLoad}
    private enum SaveMode {CompleteDataSave, JSONDataSave, SessionDataSave, NoSave}

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
                GeneralDataSaveLoader.Instance.CompleteDataLoad();
                break;
            case LoadMode.JSONDataLoad:
                GeneralDataSaveLoader.Instance.LoadPersistentData();
                break;
            case LoadMode.SessionDataLoad:
            default:
                GeneralDataSaveLoader.Instance.LoadSessionData();
                break;
            case LoadMode.NoLoad:
                break;
        }
    }

    public bool CanLoadDataDynamically() => allowDynamicLoad;

    public async void HandleDynamicDataSave() //Synchronous Methods!
    {
        switch (dynamicSaveMode)
        {
            case SaveMode.CompleteDataSave:
                await GeneralDataSaveLoader.Instance.CompleteDataLoadAsync();
                break;
            case SaveMode.JSONDataSave:
                await GeneralDataSaveLoader.Instance.SavePersistentDataAsync();
                break;
            case SaveMode.SessionDataSave:
            default:
                GeneralDataSaveLoader.Instance.SaveSessionData();
                break;
            case SaveMode.NoSave:
                break;
        }
    }

    private void HandleDataSaveOnQuit() //Synchronous Methods!
    {
        switch (applicationQuitSaveMode)
        {
            case SaveMode.CompleteDataSave:
                GeneralDataSaveLoader.Instance.CompleteDataSave();
                break;
            case SaveMode.JSONDataSave:
                GeneralDataSaveLoader.Instance.SavePersistentData();
                break;
            case SaveMode.SessionDataSave:
            default:
                GeneralDataSaveLoader.Instance.SaveSessionData();
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
