using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneralDataSaveLoader : MonoBehaviour
{
    public static GeneralDataSaveLoader Instance { get; private set; }

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
            //Debug.LogWarning("There is more than one GeneralDataSaveLoader instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }


    #region Load
    public void CompleteDataLoad()
    {
        LoadPersistentData();
        LoadSessionData();
    }
    public void LoadPersistentData()
    {
        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            dataPersistenceManager.LoadData();
        }
    }

    public void LoadSessionData()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.LoadRuntimeData();
        }
    }
    #endregion

    #region Save
    public void CompleteDataSave()
    {
        SaveSessionData();
        SavePersistentData();
    }

    public void SavePersistentData()
    {
        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            dataPersistenceManager.SaveData();
        }
    }

    public void SaveSessionData()
    {
        List<SessionDataSaveLoader> sessionDataSaveLoaders = FindObjectsOfType<SessionDataSaveLoader>().ToList();

        foreach (SessionDataSaveLoader sessionDataSaveLoader in sessionDataSaveLoaders)
        {
            sessionDataSaveLoader.SaveRuntimeData();
        }
    }
    #endregion
}
