using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public abstract class DataPersistenceManager<T> : MonoBehaviour where T : class, new()
{
    [Header("Enablers")]
    [SerializeField] private bool enableDataPersistence;
    [SerializeField] private bool enableDataSaveOnStart;
    [SerializeField] private bool enableDataSaveOnQuit;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private string dirPath;
    private IDataService dataService;
    private T persistentData;
    private List<IDataSaveLoader<T>> dataSaveLoaderObjects;

    protected void Awake()
    {
        SetSingleton();
        InitializeDataPersistenceManager();
    }

    protected void InitializeDataPersistenceManager()
    {
        dirPath = Application.persistentDataPath;

        if (useEncryption) dataService = new JSONNewtonSoftDataServiceEncryption(dirPath, fileName);
        else dataService = new JSONNewtonsoftDataServiceNoEncryption(dirPath, fileName);

        dataSaveLoaderObjects = FindAllDataSaveLoaderObjects();
        LoadGameData();
    }


    protected List<IDataSaveLoader<T>> FindAllDataSaveLoaderObjects()
    {
        IEnumerable<IDataSaveLoader<T>> dataSaveLoaderObjectsNumerable = FindObjectsOfType<MonoBehaviour>().OfType<IDataSaveLoader<T>>();
        List<IDataSaveLoader<T>> dataSaveLoaderObjectsList = new List<IDataSaveLoader<T>>(dataSaveLoaderObjectsNumerable);

        return dataSaveLoaderObjectsList;
    }

    protected virtual void SetSingleton() { }

    protected void LoadGameData()
    {
        if (!enableDataPersistence) return;

        persistentData = dataService.LoadData<T>(); //Load data from file using data handler

        if (persistentData == default || persistentData == null)
        {
            if(debug) Debug.Log("No data was found. Initializing data to defaults");

            NewGameData();
        }

        foreach (IDataSaveLoader<T> dataSaveLoaderObject in dataSaveLoaderObjects) //Push loaded data to scripts that need it
        {
            dataSaveLoaderObject.LoadData(persistentData);
        }

        if (enableDataSaveOnStart) SaveGameData();
    }

    public void SaveGameData()
    {
        if (!enableDataPersistence) return;

        foreach (IDataSaveLoader<T> dataSaveLoaderObject in dataSaveLoaderObjects) //Pass data to other scripts so they can update it
        {
            dataSaveLoaderObject.SaveData(ref persistentData);
        }

        dataService.SaveData(persistentData); //Save data to file using data handler 
    }

    protected void NewGameData() => persistentData = new T();

    public void DeleteGameData()
    {
        dirPath = Application.persistentDataPath;

        string path = Path.Combine(dirPath, fileName);

        if (!File.Exists(path))
        {
            Debug.Log("No data to delete");
            return;
        }

        File.Delete(path);
        Debug.Log("Data Deleted");
    }

    private void OnApplicationQuit() 
    {
        if (!enableDataSaveOnQuit) return;
        SaveGameData();
    }
}
