using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GeneralDataSaveLoader : MonoBehaviour
{
    public static GeneralDataSaveLoader Instance { get; private set; }

    public static event EventHandler OnDataLoadStart;
    public static event EventHandler OnDataLoadComplete;

    public static event EventHandler OnDataSaveStart;
    public static event EventHandler OnDataSaveComplete;    

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

    public async Task CompleteDataLoadAsync()
    {
        await LoadPersistentDataAsync();
        LoadSessionData();
    }

    public void LoadPersistentData()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            dataPersistenceManager.LoadData();
        }

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadPersistentDataAsync()
    {
        OnDataLoadStart?.Invoke(this, EventArgs.Empty);

        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            await dataPersistenceManager.LoadDataAsync();
        }

        OnDataLoadComplete?.Invoke(this, EventArgs.Empty);
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

    public async Task CompleteDataSaveAsync()
    {
        SaveSessionData();
        await SavePersistentDataAsync();
    }

    public void SavePersistentData()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            dataPersistenceManager.SaveData();
        }

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
    }

    public async Task SavePersistentDataAsync()
    {
        OnDataSaveStart?.Invoke(this, EventArgs.Empty);

        List<IDataPersistenceManager> dataPersistenceManagers = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceManager>().ToList();

        foreach (IDataPersistenceManager dataPersistenceManager in dataPersistenceManagers)
        {
            await dataPersistenceManager.SaveDataAsync();
        }

        OnDataSaveComplete?.Invoke(this, EventArgs.Empty);
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
